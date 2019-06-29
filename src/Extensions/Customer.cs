using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Extensions
{
    public static class Customer
    {
        public static Models.Customer ToCustomer(this DataRow userData)
        {
            //check obligatory fields
            if (userData["RUT"] == null)
                return null;

            var condosData = new List<Models.Condo>();
            userData.Table.Select().ToList().ForEach(row =>
            {
                condosData.Add(new Models.Condo
                {
                    Rut = row["RUT"].IntOrDefault(),
                    RazonSocial = row["RAZON_SOCIAL"].StringOrEmpty(),
                    Tipo = row["ID_TIPO_CONDOMINIO"].IntOrDefault(),
                    Vigencia = row["VIGENTE"].IntOrDefault()
                });
            });

            return new Models.Customer
            {
                Rut = userData["RUT"].IntOrDefault(),
                Nombre = userData["NOMBRE"].StringOrEmpty(),
                Mail = userData["EMAIL"].StringOrEmpty(),
                Icono = userData["ICONO"].StringOrEmpty(),
                Estado = userData["ID_TIPO_ESTADO_USUARIO"].IntOrDefault(),
                Tipo = userData["ID_TIPO_USUARIO_INGRESO"].IntOrDefault(),
                Condos = condosData.Where(c=> !string.IsNullOrEmpty(c.RazonSocial)).ToList()
            };
        }
    }
}
