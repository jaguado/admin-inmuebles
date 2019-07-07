using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using AdminInmuebles.Controllers;
using AdminInmuebles.Extensions;
namespace AdminInmuebles.Filters
{
    /// <summary>
    /// Attribute used to check whether the request contains a token in the Header "X-Authenticated-Userid"
    /// If it does contains the header it then will check if the body and query string of the request to find
    /// a rut, finally, it will try to find the rut in the payload of the 
    /// </summary>
    public class SocialAuth : IAsyncActionFilter
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        const string googlePublicKey = "";
        const string uidFieldName = "forUser";
        const string authHeader = "Authorization";
        const string tokenName = "access_token";
        const string providerHeader = "provider";
        private static readonly bool checkAuth = Environment.GetEnvironmentVariable("disableAuth") == null || bool.Parse(Environment.GetEnvironmentVariable("disableAuth")) == false;
        
        private bool CheckJWT(ActionExecutingContext context)
        {
            var provider = GetFromHeader(context, providerHeader);
            if (provider != null && provider != "internal" && provider != "social") return false;
            try
            {
                if (checkAuth && context.HttpContext.Request.Method != "OPTIONS")
                {
                    var anony = !context.Filters.Any(a => a is AllowAnonymousFilter);
                    //Get access token and check state
                    var accessToken = GetFromHeader(context, authHeader) ?? string.Empty;
                    if (accessToken == string.Empty)
                        accessToken = GetFromRequest(context, tokenName);
                    else
                        accessToken = accessToken.Replace("Bearer ", "");

                    //check jwt
                    var expDeltaDurationMinutes = 5;
                    var jwt = accessToken.ToJwt();
                    if (!anony && jwt==null)
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Content = "Invalid or expired token"
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Content = "Token check failed. " + ex.Message
                };
            }
            return true;
        }

        private async Task<bool> CheckGoogleAsync(ActionExecutingContext context, string accessToken, string uid)
        {
            if (GetFromHeader(context, providerHeader) != "google") return false;
            try
            {
                await ValidateAccessTokenWithGoogleAsync(context, accessToken, uid);
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Content = "GoogleAuth failed. " + ex.Message
                };
            }
            return true;
        }

        private static async Task ValidateAccessTokenWithGoogleAsync(ActionExecutingContext context, string token, string uid)
        {
            //check if token is valid 
            const string baseUrl = "https://oauth2.googleapis.com/tokeninfo?id_token=";
            using (var response = await Helpers.Net.GetResponse(baseUrl + token))
            {
                if (!response.IsSuccessStatusCode)
                    context.Result = new ContentResult()
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Content = "Invalid access token"
                    };
            }
        }

        private async Task<bool> CheckFacebookAsync(ActionExecutingContext context, string accessToken, string uid)
        {
            if (GetFromHeader(context, providerHeader) != "facebook") return false;
            try
            {
                await ValidateAccessTokenWithFacebookAsync(context, accessToken, uid);
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Content = "FacebookAuth failed. " + ex.Message
                };
            }
            return true;
        }

        private static async Task ValidateAccessTokenWithFacebookAsync(ActionExecutingContext context, string token, string uid)
        {
            //check if token is valid
            using (var response = await Helpers.Net.GetResponse("https://graph.facebook.com/me?access_token=" + token))
            {
                if (!response.IsSuccessStatusCode)
                    context.Result = new ContentResult()
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Content = "Invalid access token"
                    };
            }
        }

        private T GetFromBody<T>(ActionExecutingContext context, string key)
        {
            return context.ActionArguments.ContainsKey(key) ? (T)context.ActionArguments[key] : default(T);
        }

        private string GetFromRequest(ActionExecutingContext context, string key)
        {
            return context.ActionArguments.ContainsKey(key) ? context.ActionArguments[key].ToString() : context.HttpContext.Request.Query[key].ToString();
        }

        private string GetFromHeader(ActionExecutingContext context, string headerName = "X-Authenticated-Userid")
        {
            return context.HttpContext.Request.Headers[headerName];
        }

        private bool JwtContains(JwtSecurityToken token, string key, string value)
        {
            try
            {
                var tokenValue = token.Payload[key].ToString();
                var auth = tokenValue == value;
                if (!auth) Console.Error.WriteLineAsync($"Authorization problem on field '{key}'. Input: {value.ToString()}, Auth: {tokenValue.ToString()}");
                return auth;
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                Console.Error.WriteLineAsync(ex.Message);
                return false;
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //remove auth when method is OPTIONS 
            if (context.HttpContext.Request.Method != "OPTIONS")
            {
                //Get access token and check state
                var accessToken = GetFromHeader(context, authHeader) ?? string.Empty;
                if (accessToken == string.Empty)
                    accessToken = GetFromRequest(context, tokenName);
                else
                    accessToken = accessToken.Replace("Bearer ", "");

                if (string.IsNullOrEmpty(accessToken))
                {
                    //throw new SecurityTokenException("Access token missing");
                    // Check for authorization
                    if (!context.Filters.Any(a=> a is AllowAnonymousFilter) && checkAuth)
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Content = "Access token missing"
                        };
                    }
                }
                else
                {
                    var uid = GetFromRequest(context, uidFieldName);
                    var jwt = CheckJWT(context);
                    var google = await CheckGoogleAsync(context, accessToken, uid);
                    var facebook = await CheckFacebookAsync(context, accessToken, uid);
                    if (context.Controller is BaseController controller)
                        controller.AuthenticatedToken = accessToken.ToString().ToJwt(!jwt);
                }
            }

            if (context.Result == null)
                await next();
        }
    }
}
