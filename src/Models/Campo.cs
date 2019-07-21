using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Models
{
    public class Campo
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public bool Opcional { get; set; }

        public int Largo { get; set; }
        public int Precision { get; set; }

        public bool IsIdentity { get; set; }
    }
}
