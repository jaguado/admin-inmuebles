using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class Credentials
    {
        public string email { get; set; }
        public string password { get; set;  }

        public enum Types
        {
            Social = 1,
            System = 2
        }
    }
}
