using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Repository
{
    public class CustomerRepository
    { 
        public async Task<Models.Customer> Get(string email)
        {
            var args = new Dictionary<string, string>
            {
                { "MAIL", email }
            };
            //TODO map to entity
            var dsResult = await Helpers.Sql.Execute("[desoincl_inmueble].[SP_TRAER_USUARIO]", args);
            if (dsResult != null)
            {
                var userData = dsResult.Tables[0].Select()[0];
                return new Models.Customer
                {
                    Rut= int.Parse(userData["RUT"].ToString()),
                    Nombre= userData["NOMBRE"].ToString(),
                    Mail= userData["EMAIL"].ToString(),
                    Icono = userData["ICONO"].ToString(),
                    Estado = int.Parse(userData["ID_TIPO_ESTADO_USUARIO"].ToString()),
                    Tipo = int.Parse(userData["ID_TIPO_USUARIO_INGRESO"].ToString())
                };
            }
            return null;
        }
        public async Task<bool> Create(Models.Customer customer)
        {
            var args = new Dictionary<string, string>
            {
                { "MAIL", customer.Mail },
                { "NOMBRE", customer.Nombre },
                { "RUT", customer.Rut.ToString() },
                { "TIPO_ESTADO_USUARIO", customer.Estado.ToString() },
                { "TIPO_USUARIO_INGRESO", customer.Tipo.ToString() },
                { "ICONO", customer.Icono ?? ""},
                { "CLAVE", customer.Password }
            };
            return await Helpers.Sql.ExecuteScalar("[desoincl_inmueble].[SP_USUARIO_CREAR]", args) > -1;
        }
        public async Task<bool> UpdateAsync(Models.Customer customer)
        {
            return await Create(customer);
        }
        public async Task<DataSet> CheckPassword(Models.Credentials credentials)
        {
            var args = new Dictionary<string, string>
            {
                { "MAIL", credentials.email },
                { "PASS", credentials.password }
            };
            return await Helpers.Sql.Execute("[desoincl_inmueble].[SP_USUARIO_VALIDA_CREDENCIALES]", args);
        }
        public async Task<bool> UpdatePassword(Models.Credentials credentials)
        {
            var args = new Dictionary<string, string>
            {
                { "MAIL", credentials.email },
                { "CLAVE", credentials.password }
            };
            return await Helpers.Sql.ExecuteScalar("[desoincl_inmueble].[SP_USUARIO_CAMBIO_CLAVE]", args) > 0;
        }
    }
}
