using AdminInmuebles.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AdminInmuebles.Extensions;
using System.Threading;
using AdminInmuebles.Filters;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace AdminInmuebles.Controllers
{
    [Route("v1/[controller]")]
    public class WebhookController : BaseController
    {
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> WebHookAsync()
        {
            //TODO check auth 

            //send request data to console
            var request = HttpContext.Request;
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            Console.WriteLine("Webhook body: " + body);
            return new OkResult();
        }    
    }
}
