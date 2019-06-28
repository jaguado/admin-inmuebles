using AdminInmuebles.Extensions;
using AdminInmuebles.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Controllers
{
    [Route("v1/[controller]")]
    public class CustomerController : BaseController
    {
        private readonly Repository.CustomerRepository _customerRepository = new Repository.CustomerRepository();
        [HttpGet()]
        [Produces(typeof(Models.Customer))]
        public async Task<IActionResult> Get()
        {
            var loggedCustomer = getLoggedCustomer();
            if (loggedCustomer == null)
                return new UnauthorizedResult();

            //complete customer data
            var result = await _customerRepository.Get(loggedCustomer.Mail);
            if (result == null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        [HttpPost()]
        [Produces(typeof(Models.Customer))]
        public async Task<IActionResult> Create([FromBody] Models.Customer customer)
        { 
            if(customer==null)
                return new BadRequestResult();
            var loggedCustomer = getLoggedCustomer();
            if (loggedCustomer == null)
                return new UnauthorizedResult();

            if (!loggedCustomer.Mail.ToLower().Equals(customer.Mail.ToLower()))
                return new ForbidResult();

            //create customer on DB
            var result = await _customerRepository.Create(customer);
            if (result)
            {
                customer.Password = null;
                return new OkObjectResult(customer);
            }

            return new StatusCodeResult(304); //not modified
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword()
        {
            var loggedCustomer = getLoggedCustomer();
            if (loggedCustomer == null)
                return new UnauthorizedResult();

            if (string.IsNullOrEmpty(loggedCustomer.Mail))
                return new NotFoundObjectResult(loggedCustomer);

            var newPassword = Password.CreateWithRandomLength();
            var destination = new List<SendGrid.Helpers.Mail.EmailAddress> { new SendGrid.Helpers.Mail.EmailAddress(loggedCustomer.Mail, loggedCustomer.Nombre) };
            var payload = new
            {
                name = loggedCustomer.Nombre,
                password = newPassword
            };
            var result = await _customerRepository.UpdatePassword(new Models.Credentials { email = loggedCustomer.Mail, password = newPassword });
            if (result)
            {
                var mail = await Email.SendTransactional(destination, Email.Templates.Transactional.PasswordReset , payload);
                if (mail.StatusCode == System.Net.HttpStatusCode.Accepted)
                    return new OkResult();
                else
                    return new StatusCodeResult((int)mail.StatusCode);
            }
            return new StatusCodeResult(304); //not modified        
        }

        private Models.Customer getLoggedCustomer()
        {
            if (AuthenticatedToken == null || !AuthenticatedToken.Payload.ContainsKey("email"))
                return null;

            return new Models.Customer
            {
                Mail = AuthenticatedToken.Payload["email"].ToString(),
                Nombre = AuthenticatedToken.Payload["name"].ToString()
            };
        }
    }
}
