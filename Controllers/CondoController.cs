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
    public class CondoController:BaseController
    {
        [HttpGet()]
        public async Task<IActionResult> GetCondos()
        {
            return new OkObjectResult(await MongoDB.FromMongoDB<Models.Condo>());
        }
        [HttpPost()]
        public async Task<IActionResult> CreateCondo([FromBody] Models.Condo user)
        {
            return await user.ToMongoDB<Models.Condo>();
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateCondo([FromBody] Models.Condo user)
        {
            return await user.ToMongoDB<Models.Condo>(true);
        }
    }
}
