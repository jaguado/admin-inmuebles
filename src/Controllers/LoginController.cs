using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AdminInmuebles.Extensions;
using AdminInmuebles.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
                //TODO create customer when not exists
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

            //TODO return real JWT
            var jwt = GetJWT(customer);
            return new OkObjectResult(new
            {
                email = customer.Mail,
                firstName = customer.Nombre,
                idToken = jwt,
                name = customer.Nombre,
                photoUrl = customer.Icono,
                provider = customer.Tipo == (int) Models.Credentials.Types.Social ? "social" : "internal",
                state = customer.Estado,
                data = customer.Condos
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



        private static byte[] privateKey = Convert.FromBase64String("LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpNSUlFcEFJQkFBS0NBUUVBNGhxeUNvdDFQMm43YkpJaUkycmRaUTdybmRscThwb0lkU296TjloeUJXeTJNaFc2CmJIV0IvRXg2VmVxdFBZa3k1RjFHT1hudFZwdmY1OGhmTTV5cmMxOE1ST0NzemRmVUVoeTZyUHkrRCtUOFkvYXgKVkQ1aFE1aG1aOWhwSTd0azJtRHRsa2g0Z1JEZ2puV0VPMU15NU9UcUIrNkZzY243d0RRUmpWS0hXdDAvWVpzZgpYclVWS0dORzl2UjY1bVFQd2NOVVpQbEkxSmN4OFBtZzE3TUwrcXp5Rzg2QXJwdnpLaFRRRnpiTDFXSWd1OEc5Ck5kSTdtalBYT2IzZG1YU1QvSFY1dlhLbmdGLzlOTGJZNXFIUHFmaVFma0Q3Q2ZUOVZTUzV3NUNWRkJVbzFwbk0KaWZuTU1zamhTcmxybUI3dW5aZXdsU3BZZnFBYm9MWXRvbElFTFFJREFRQUJBb0lCQUcySEp3WUtwbUp3c1pYWAplR2dWeFdmR0FZYzJvaU1oVU1XSkNzU2J1aWc4a0VuVTByamxkM0ZEanVOOXlpd01BVFkwcE9jRTFGN25KV0MvClpMYTR4eWtkT3ZGR3NROEo1VFpjNm1VUURmWGZKQkE4bVl4SXA0ODZEU2x5NFFPcExHTEpIMjUwYnNOKzdIaTIKSHJjcVIzWXdHZHA3eGhIbzJXWXpFdW1WdC9IZ1gvazl4Y2hPVDF3dzU1M0JnelV0RnBHaHlZak1BS3dEQXRiSQpwY2dkd3RZczdEUFBDR3NmZHA1VjI0THJjbzYyN1BpZXE3cEdoaFNheCtTYmQ3bjA4ZXpQVldxQmdZSjlnWWFFCkVvZGtnL2ZlNXc2S0dEZkFwZFFETkRWUTNlSE9kUXd1enZVdHNvNHo0ZVBrT2xza0lESmR1WUIrNWo3bkVqbmQKelQ3WVhQVUNnWUVBL2lrejRCWFdEWVgvdlpmc3JLZWg4bHNjMDNYTzRnTmFNN0hwL253bG50ajJ5ck9hODIyUQppKzUzdTFpbGVEY25mWEdJd0U0VUJKSWtaZERqM3MyNEVCbUx2My9KdSs1dFlWcDJyMGtDSnRibWE3SU5CZ1Z5ClRuZWNwaXBjQmt2V3AzMHgzU0pCK0ZDQjZYcE1wZ2M1RmlpZWlNWHZwOVM2M2paMEhTZXZPU3NDZ1lFQTQ3MkYKbFRZZE9tNlR1aWJQRi9OTGxuanh0b05mZzRoQmVsRWd4ZS9UQ29NeU9sVzNwRzJVNUhDOTZPbGc1RklpdFFpcQp6QVBnSExQSmdwY2VVeW5aNG1NeWVoUXdSenNYeGthWk5kSUx1QXUyTTQ4UDltUEN1SUVZc3IyU2ZhWThrQ1l0Cm1kRUtvdkllZEhrN0JrSXZBMmlZbGNYcm9wZ1FYelBrV3NYMFhBY0NnWUVBbno5andJa09DSVVvT3p3QTBDRnMKaURUc2Q5WTkzVUVxZUcrR1pLeVd5ZE81dGtJWTJXT1NDUXRPdS9VTUlLbTJOWlE0a2YyWjcwOCtQUWxJYmFiSwpLRlJKU3FDZjN2L0NTeGhxZXVPczFIY2NBdWlaM21iMU94TVk5TWhmeHBZb0ZlT01wYmk4U3dEdWxVTEsyZEIyCnhWcUlFcnlxcjZiTHUwVzFOVHRUUCtjQ2dZRUFrN0REb1psZmFSWnUzU2p2NHFOWUlMUThaTlZicXN4QlVsYXMKZjFEaE53OFFFcjZtQW85Q0lNZHdrVXhRRnFHaGVtK3RlL01EY3ZteE0reFdzUzRSNi92U3IxTEtZRmRWT3JOcwpCbmc2TzFmMUNBaStIRlpqNEExd2UvSHV2MmVBSFNkMUtTeGt6bmxnQUw3aDVWUWtjdnh3LzZoRUFNVEcwVWF3Cno2RlNzQjhDZ1lCUFNaOG9kK1l3di93bTZNN3p3YWRjN1NscGRUeHhWUkt2YklCQ0ZlbDFucHJCc3kyM1pTdFUKU3lPOTI4QjZoY0hScEdSVVN5V2I5UkgvK0Q2TFJsL3UyRjJIMHhXc21rYzdBZjR6d3RxUEdqWE1qTThZeGRlMwpZUWdlTjJ3cGpHdGc2aENWRnVyZzR2Unpxd3VqMGFvWkhBeFIzcTBEVkJCVWp4TUxnSG03bHc9PQotLS0tLUVORCBSU0EgUFJJVkFURSBLRVktLS0tLQ==");
        private static byte[] publicKey = Convert.FromBase64String("c3NoLXJzYSBBQUFBQjNOemFDMXljMkVBQUFBREFRQUJBQUFCQVFEaUdySUtpM1UvYWZ0c2tpSWphdDFsRHV1ZDJXcnltZ2gxS2pNMzJISUZiTFl5RmJwc2RZSDhUSHBWNnEwOWlUTGtYVVk1ZWUxV205L255Rjh6bkt0elh3eEU0S3pOMTlRU0hMcXMvTDRQNVB4ajlyRlVQbUZEbUdabjJHa2p1MlRhWU8yV1NIaUJFT0NPZFlRN1V6TGs1T29IN29XeHlmdkFOQkdOVW9kYTNUOWhteDlldFJVb1kwYjI5SHJtWkEvQncxUmsrVWpVbHpIdythRFhzd3Y2clBJYnpvQ3VtL01xRk5BWE5zdlZZaUM3d2IwMTBqdWFNOWM1dmQyWmRKUDhkWG05Y3FlQVgvMDB0dGptb2MrcCtKQitRUHNKOVAxVkpMbkRrSlVVRlNqV21jeUorY3d5eU9GS3VXdVlIdTZkbDdDVktsaCtvQnVndGkyaVVnUXQgcXNhbmRib3g=");

        private static string GetJWT(Models.Customer customer)
        {  
            const int defaultTokenDuration = 5;
            var expirationDate = DateTime.Now.AddMinutes(defaultTokenDuration);
            var claims = new List<Claim>
                {
                    new Claim("iss", "AdminInmuebles." + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")),
                    new Claim("email", customer.Mail),
                    new Claim("name", customer.Nombre)
                };
            //TODO add cert signing
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                claims: claims,
                expires: expirationDate));
        }
    }
}
