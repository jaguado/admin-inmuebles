using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Property : BaseModel
    {
        public User Owner { get; set; }
        public User[] Habitants { get; set; }
        public User[] RelatedPersons { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public Address Address { get; set; }

        public enum PropertyTypes
        {
            Home, Apartment, Office, Loft
        }
    }
}
