using AdminInmuebles.Extensions;
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
                return dsResult.Tables[0].Select()[0].ToCustomer();
            return null;
        }
        public async Task<bool> CreateOrUpdate(Models.Customer customer)
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
            var result = await Helpers.Sql.ExecuteScalar("[desoincl_inmueble].[SP_USUARIO_CREAR]", args);
            return result > -1;
        }
        public async Task<bool> UpdateAsync(Models.Customer customer)
        {
            return await CreateOrUpdate(customer);
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
            var res = await Helpers.Sql.ExecuteScalar("[desoincl_inmueble].[SP_USUARIO_CAMBIO_CLAVE]", args);
            return res > 0;
        }
    }
}
