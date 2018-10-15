using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class BaseModel
    {
        public User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public User UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
