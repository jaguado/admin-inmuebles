using AdminInmuebles.Extensions;
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
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            if (this.AuthenticatedToken == null)
                return new ForbidResult();

            //get logged user information
            throw new NotImplementedException();
        }
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] Models.Customer customer)
        {
            throw new NotImplementedException();
        }
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Models.Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
