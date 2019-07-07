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

            NewRelic.Api.Agent.NewRelic.AddCustomParameter("customer.email", loggedCustomer.Mail);
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
            NewRelic.Api.Agent.NewRelic.AddCustomParameter("customer.email", customer.Mail);
            if (customer==null)
                return new BadRequestResult();
            var loggedCustomer = getLoggedCustomer();
            if (loggedCustomer == null)
                return new UnauthorizedResult();

            if (!loggedCustomer.Mail.ToLower().Equals(customer.Mail.ToLower()))
                return new ForbidResult();

            // if rut is present change state to active
            if (customer.Rut > 0 && customer.Estado == 0)
                customer.Estado = 1;

            //create customer on DB
            var result = await _customerRepository.CreateOrUpdate(customer);
            if (result)
            {
                customer.Password = null;
                return new OkObjectResult(customer);
            }

            return new StatusCodeResult(304); //not modified
        }

        private Models.Customer getLoggedCustomer()
        {
            if (AuthenticatedToken == null || !AuthenticatedToken.Payload.ContainsKey("email"))
                return null;

            NewRelic.Api.Agent.NewRelic.AddCustomParameter("customer.email", AuthenticatedToken.Payload["email"].ToString());
            return new Models.Customer
            {
                Mail = AuthenticatedToken.Payload["email"].ToString(),
                Nombre = AuthenticatedToken.Payload["name"].ToString()
            };
        }
    }
}
