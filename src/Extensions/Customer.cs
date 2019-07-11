using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Extensions
{
    public static class Customer
    {
        private static Models.Customer ToCustomer(this DataRow userData)
        {
            //check obligatory fields
            if (userData["RUT"] == null)
                return null;

            var condosData = new List<Models.Condo>();
            userData.Table.Select().ToList().ForEach(row =>
            {
                condosData.Add(new Models.Condo
                {
                    Id = row["ID_CONDOMINIO"].IntOrDefault(),
                    Rut = row["RUT"].IntOrDefault(),
                    RazonSocial = row["RAZON_SOCIAL"].StringOrEmpty(),
                    Tipo = row["ID_TIPO_CONDOMINIO"].IntOrDefault(),
                    Vigencia = row["VIGENTE"].IntOrDefault(),
                    Roles = new List<Models.Customer.Roles> {(Models.Customer.Roles) row["Role"].IntOrDefault() }
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

        public static Models.Customer ToCustomer(this DataRow[] userData)
        {
            var results = userData.Select(dr => dr.ToCustomer()).ToList();
            var resultantCustomer = results.First();
            resultantCustomer.Condos = new List<Models.Condo>();
            results.ForEach(customer =>
            {
                customer.Condos.ForEach(condo =>
                {
                    var currentCondo = resultantCustomer.Condos.FirstOrDefault(c => c.Id == condo.Id);
                    if (currentCondo != null)
                        currentCondo.Roles.AddRange(condo.Roles.Where(r => !currentCondo.Roles.Any(r1 => r1 == r)));
                    else
                        resultantCustomer.Condos.Add(condo);
                });     
            });
            return resultantCustomer;
        }
    }
}
