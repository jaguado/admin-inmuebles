﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class AuthenticatedUser: BaseModel
    {
        public Customer User { get; set; }
        public List<MenuSection> Menu { get; set; }
    }
}
