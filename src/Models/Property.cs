using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Property : BaseModel
    {
        public Customer Owner { get; set; }
        public Customer[] Habitants { get; set; }
        public Customer[] RelatedPersons { get; set; }
        public PropertyTypes PropertyType { get; set; }
        public Address Address { get; set; }

        public enum PropertyTypes
        {
            Home, Apartment, Office, Loft
        }
    }
}
