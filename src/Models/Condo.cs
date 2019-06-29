using System;
using System.Collections.Generic;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Condo : BaseModel
    {
        public int Rut { get; set; }
        public string RazonSocial { get; set; }
        public int Tipo { get; set; }
        public int Vigencia { get; set; }
    }
}
