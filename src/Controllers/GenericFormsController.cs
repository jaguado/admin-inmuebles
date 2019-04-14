using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Controllers
{

    [Route("v1/[controller]")]
    [AllowAnonymous]
    public class GenericFormsController : BaseController
    {
        [HttpGet()]
        [Produces(typeof(IEnumerable<Models.Tabla>))]
        public async Task<IActionResult> GetTypesTables()
        {
            const string queryMantenedores = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME like 'TIPO_%'";
            var tables = await Helpers.Sql.GetData(queryMantenedores);
            var rows = tables.Tables[0].Select().ToList();
            var output = rows.Select(row =>
            {
                return new Models.Tabla
                {
                    BD = row["TABLE_CATALOG"].ToString(),
                    Esquema = row["TABLE_SCHEMA"].ToString(),
                    Nombre = row["TABLE_NAME"].ToString()
                };
            });
            return new OkObjectResult(output);
        }

        [HttpGet("{tableName}")]
        [Produces(typeof(IEnumerable<Models.Campo>))]
        public async Task<IActionResult> GetTableDetail(string tableName)
        {
            var queryTabla = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = '{tableName}'";
            var tables = await Helpers.Sql.GetData(queryTabla);
            var rows = tables.Tables[0].Select().ToList();
            var output = rows.Select(row =>
            {
                return new Models.Campo
                {
                    Nombre = row["COLUMN_NAME"].ToString(),
                    Tipo = row["DATA_TYPE"].ToString(),
                    Opcional = row["DATA_TYPE"] != null && row["DATA_TYPE"].ToString() == "NO"
                };
            });
            return new OkObjectResult(output);
        }

        [HttpPost("{tableName}")]
        public Task<IActionResult> AddData(string tableName)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{nombre}")]
        public Task<IActionResult> UpdateData(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}
