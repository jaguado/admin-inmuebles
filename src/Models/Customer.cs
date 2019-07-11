using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdminInmuebles.Models
{
    public class Customer : BaseModel
    {
        public int Rut { get; set; }
        public string Mail { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public int Estado { get; set; }
        public string Icono { get; set; }
        public string Password { get; set; }

        public static implicit operator Customer(DataSet v)
        {
            throw new NotImplementedException();
        }

        public List<Condo> Condos { get; set; }

        public enum Roles
        {
            God = 1,
            User = 2,
            Admin = 7
        }
    }
}
