﻿using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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


        [HttpGet("me")]
        public IActionResult GetAuthUser(string forUser, string userInfo)
        {
            //TODO get real condos, menus and roles info
            return new OkObjectResult(new
            {
                User = JsonConvert.DeserializeObject(userInfo),
                Condos = new List<object>(5),
                Roles = new List<object>(5),
                Menus = new List<object>(5)
            });
        }
    }
}
