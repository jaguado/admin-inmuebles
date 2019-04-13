using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
        public string AttachementName { get; set; }
        public string AttachementContent { get; set; }
    }
}
