using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Condo : BaseModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Address Address { get; set; }
        public Property[] Properties { get; set; }
        public Service[] Services { get; set; }
        public User[] Persons { get; set; }
    }
}
