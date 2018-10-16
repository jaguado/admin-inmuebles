using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Controllers
{
    [Route("v1/[controller]")]
    public class UserController:BaseController
    {
        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            return new OkObjectResult(await MongoDB.FromMongoDB<Models.User>());
        }
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] Models.User user)
        {
            return await user.ToMongoDB<Models.User>();
        }
        [AllowAnonymous]
        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] Models.User user)
        {
            return await user.ToMongoDB<Models.User>();
        }
    }
}
