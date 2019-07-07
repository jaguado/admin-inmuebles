using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AdminInmuebles.Helpers
{
    public static class Jwt
    {
        private static readonly bool _checkJwtSignature = Environment.GetEnvironmentVariable("disableJwtCheck") == null || bool.Parse(Environment.GetEnvironmentVariable("disableJwtCheck")) == false;
        private static readonly string _certPath = Environment.GetEnvironmentVariable("JwtCertificatePath") ?? @"Certs/adminmuebles.pfx";
        private static readonly byte[] _rawCert = System.IO.File.Exists(_certPath) ? System.IO.File.ReadAllBytes(_certPath) : System.Text.ASCIIEncoding.Default.GetBytes(_certPath);
        internal static X509Certificate2 Cert = !_checkJwtSignature ? new X509Certificate2(_rawCert, Environment.GetEnvironmentVariable("JwtCertificatePassword") ?? "AdmInmuebles.2019") : null;
        internal static X509SigningCredentials Creds = !_checkJwtSignature ? new X509SigningCredentials(Cert, "RS256") : null;
        public static string Create(Models.Customer customer, int tokenDuration)
        {
            var expirationDate = DateTime.Now.AddMinutes(tokenDuration);
            var claims = new List<Claim>
                {
                    new Claim("iss", $"AdmInmuebles.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}"),
                    new Claim("email", customer.Mail),
                    new Claim("name", customer.Nombre),
                    new Claim("data", JsonConvert.SerializeObject(customer))
                };
            //TODO add cert signing
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                claims: claims,
                expires: expirationDate,
                signingCredentials: Creds));
        }

        public static JwtSecurityToken ValidateAndDecode(string jwt, bool skipSignatureValidation=false)
        {
            if (Creds == null || skipSignatureValidation || !_checkJwtSignature) return new JwtSecurityToken(jwt);

            var validationParameters = new TokenValidationParameters
            {
                // Clock skew compensates for server time drift.
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(5),
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Creds?.Key
            };

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwt, validationParameters, out var rawValidatedToken);

                return (JwtSecurityToken)rawValidatedToken;
                // Or, you can return the ClaimsPrincipal
                // (which has the JWT properties automatically mapped to .NET claims)
            }
            catch (SecurityTokenValidationException stvex)
            {
                // The token failed validation!
                // TODO: Log it or display an error.
                // throw new Exception($"Token failed validation: {stvex.Message}");
            }
            catch (ArgumentException argex)
            {
                // The token was not well-formed or was invalid for some other reason.
                // TODO: Log it or display an error.
                // throw new Exception($"Token was invalid: {argex.Message}");
            }
            return null;
        }
    }

}
