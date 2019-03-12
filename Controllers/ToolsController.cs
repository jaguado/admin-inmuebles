using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Controllers
{
    [Route("v1/[controller]")]
    public class ToolsController : BaseController
    {
        [HttpPost("mail")]
        public async Task<IActionResult> SendEmail(Models.Email email)
        {
            var response = await Helpers.Email.Send(email.From, email.To, email.Subject, email.PlainTextContent, email.HtmlContent, email.AttachementName, email.AttachementContent);
            var body = await response.Body.ReadAsStringAsync();
            return new ContentResult() { Content=body, StatusCode = (int) response.StatusCode };
        }

        [HttpGet("document/{id}")]
        [Produces(typeof(Models.Document))]
        public IActionResult GetDocument(string id)
        {
            //TODO get document from db
            return new OkObjectResult(new Models.Document());
        }
        [HttpPost("document")]
        public IActionResult SaveDocument(Models.Document document)
        {
            //TODO save document
            return new NoContentResult();
        }
    } 
}
