﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class Section
    {
        public string Name { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
