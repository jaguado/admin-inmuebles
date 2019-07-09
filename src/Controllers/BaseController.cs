using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using AdminInmuebles.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminInmuebles.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Allows cors support
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult Options()
        {
            return new OkResult();
        }

        internal IActionResult HandleException(Exception ex)
        {
            Log(ex);
            return StatusCode(500, ex.Message);
        }

        internal IActionResult HandleWebException(WebException ex)
        {
            Log(ex);
            return StatusCode((int)ex.Status, ex.Message);
        }

        internal void Log(Exception ex)
        {
            var msg = $"{DateTime.Now.ToString()}|{JsonConvert.SerializeObject(ex)}";
            Console.Error.WriteLineAsync(msg);
        }


        //TODO Return validated jwt object
        public JwtSecurityToken AuthenticatedToken { get; set; }

        internal bool IsAdminAtLeast()
        {
            if (AuthenticatedToken != null &&
                AuthenticatedToken.Payload.ContainsKey("roles")
                && AuthenticatedToken.Payload["roles"] is string data
                && JsonConvert.DeserializeObject<string[]>(data) is string[] roles)
                    return roles != null && roles.Any(r => r == Models.Customer.Roles.Admin.ToString() || r == Models.Customer.Roles.God.ToString());
            return false;
        }
    }
}