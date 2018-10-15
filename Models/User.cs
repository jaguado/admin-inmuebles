using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class User : BaseModel
    {
        public Address Address { get; set; }
        public Roles Role { get; set; }
        public enum Roles
        {

        }
    }
}
