using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class Menu
    {
        public string Name { get; set; }
        public string IconClass { get; set; }
        public List<Option> Options { get; set; }

        public class Option
        {
            public string Title { get; set; }
            public string Href { get; set; }
            public string BreadcrumbLabel { get; set; }
            public List<Option> Options { get; set; }
        }
    }
}
