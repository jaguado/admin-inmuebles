using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Condo : BaseModel
    {
        public Property[] Properties { get; set; }
        public Address Address { get; set; }
        public User[] RelatedPersons { get; set; }
    }
}
