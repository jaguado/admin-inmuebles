using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class BaseModel
    {
        public Customer CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Customer UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public KeyValuePair<string, object> Attributes { get; set; }
    }
}
