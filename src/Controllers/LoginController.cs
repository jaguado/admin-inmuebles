using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AdminInmuebles.Extensions;
using AdminInmuebles.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AdminInmuebles.Controllers
{
    [AllowAnonymous]
    [Route("v1/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly Repository.CustomerRepository _customerRepository = new Repository.CustomerRepository();

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("/health")]
        public IActionResult Get()
        {
            return new OkResult();
        }

        /// <summary>
        /// Login endpoint (doesn't check auth headers)
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>Valid JWT with authorization and customer information to load home screen</returns>
        [HttpPost("/login")]
        public async Task<IActionResult> PostAsync([FromBody] Models.Credentials credentials)
        {
            Models.Customer customer;
            if (AuthenticatedToken != null) //social auth
            {
                customer = await _customerRepository.Get(AuthenticatedToken.Payload["email"].ToString());
                if (customer == null)
                {
                    customer = new Models.Customer
                    {
                        Nombre = AuthenticatedToken.Payload["name"].ToString(),
                        Mail = AuthenticatedToken.Payload["email"].ToString(),
                        Tipo = (int)Models.Credentials.Types.Social, //social user
                        Estado = 2, //initial state
                        Condos = new List<Models.Condo>()
                    };
                    if(!await _customerRepository.CreateOrUpdate(customer))
                        return new BadRequestObjectResult(customer); //problems creating customer on db
                }
            }
            else
            {
                var getUserInfo = await _customerRepository.CheckPassword(credentials);
                if ((getUserInfo == null || getUserInfo.Tables.Count == 0 || getUserInfo.Tables[0].Rows.Count == 0))
                    return new UnauthorizedResult();
                customer = getUserInfo.Tables[0].Select()[0].ToCustomer();
            }

            if (customer.Estado > 2)
                return new ForbidResult(); //user disabled

            var defaultDuration = !Request.Query.TryGetValue("tokenDuration", out StringValues customTokenDuration);
            var tokenDuration = defaultDuration ? 5 : int.Parse(customTokenDuration);
            var jwt = Jwt.Create(customer, tokenDuration);
            return new OkObjectResult(new
            {
                email = customer.Mail,
                firstName = customer.Nombre,
                idToken = jwt,
                name = customer.Nombre,
                photoUrl = customer.Icono,
                provider = customer.Tipo == (int) Models.Credentials.Types.Social ? "social" : "internal",
                state = customer.Estado,
                data = customer.Condos,
                validTo = DateTime.Now.AddMinutes(tokenDuration).ToUniversalTime().ToString()
            });
        }

        /// <summary>
        /// Reset Customer Password
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("/v1/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] Models.Credentials credentials)
        {
            //TODO add some abuse prevention mechanism

            var customer = await _customerRepository.Get(credentials.email);
            if (customer == null)
                return new BadRequestObjectResult("If you continue having problemas please contact us!!");

            if(customer.Tipo == (int)Models.Credentials.Types.Social)
                return new BadRequestObjectResult("Invalid option, you cannot reset your password from here!!");

            var destination = new List<SendGrid.Helpers.Mail.EmailAddress> { new SendGrid.Helpers.Mail.EmailAddress(credentials.email, credentials.email) };
            var payload = new
            {
                name = credentials.email,
                password = Password.CreateWithRandomLength()
            };
            var result = await _customerRepository.UpdatePassword(new Models.Credentials { email = payload.name, password = payload.password });
            if (result)
            {
                var mail = await Email.SendTransactional(destination, Email.Templates.Transactional.PasswordReset, payload);
                if (mail.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return new OkResult();
                else
                {
                    var error = await mail.Body.ReadAsStringAsync();
                    return new ObjectResult(error) { StatusCode = (int)mail.StatusCode };
                }
            }
            return new StatusCodeResult(304); //not modified        
        }

        /// <summary>
        /// Customer OnBoarding
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("/v1/newCustomer")]
        public async Task<IActionResult> NewCustomer([FromBody] Models.Credentials credentials)
        {
            // TODO add some abuse prevention mechanism

            // check if customer exists
            if (await _customerRepository.Get(credentials.email) != null)
                return new BadRequestObjectResult("If you continue having problemas please contact us !!");

            var newCustomer = new Models.Customer
            {
                Nombre = credentials.email.StringWithoutDomain(),
                Mail = credentials.email,
                Tipo = (int) Models.Credentials.Types.System,
                Estado = 2, //initial state
                Condos = new List<Models.Condo>(),
                Password = Password.CreateWithRandomLength()
            };
            if (!await _customerRepository.CreateOrUpdate(newCustomer))
                return new BadRequestObjectResult(newCustomer); //problems creating customer on db

            var destination = new List<SendGrid.Helpers.Mail.EmailAddress> { new SendGrid.Helpers.Mail.EmailAddress(newCustomer.Mail, newCustomer.Nombre) };
            var payload = new
            {
                name = newCustomer.Nombre,
                password = newCustomer.Password
            };
            var mail = await Email.SendTransactional(destination, Email.Templates.Transactional.NewCustomer, payload);
            if (mail.StatusCode == System.Net.HttpStatusCode.Accepted)
                return new OkResult();
            else
            {
                var error = await mail.Body.ReadAsStringAsync();
                return new ObjectResult(error) { StatusCode = (int)mail.StatusCode };
            }
        }
    }
}
