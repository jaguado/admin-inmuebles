using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Customer : BaseModel
    {
        public string Username { get; set; }
        public Person Person { get; set; }
        public Address Address { get; set; }
        public RoleTypes[] Roles { get; set; }
        public enum RoleTypes
        {
            Other, Admin, Owner, OwnerFamily, Lessee, LesseeFamily, Worker
        }
        public dynamic Info { get; set; }
    }
}
