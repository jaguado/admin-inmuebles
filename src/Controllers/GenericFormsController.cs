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
        public async Task<IActionResult> GetTypesTables()
        {
            const string queryMantenedores = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME like 'TIPO_%'";
            var tables = await Helpers.Sql.GetData(queryMantenedores);
            var rows = tables.Tables[0].Select().ToList();
            var output = rows.Select(row =>
            {
                return new
                {
                    BD = row["TABLE_CATALOG"],
                    Esquema = row["TABLE_SCHEMA"],
                    Tabla = row["TABLE_NAME"]
                };
            });
            return new OkObjectResult(output);
        }

        [HttpGet("{tableName}")]
        public async Task<IActionResult> GetTableDetail(string tableName)
        {
            var queryTabla = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = '{tableName}'";
            var tables = await Helpers.Sql.GetData(queryTabla);
            var rows = tables.Tables[0].Select().ToList();
            var output = rows.Select(row =>
            {
                return new
                {
                    Nombre = row["COLUMN_NAME"],
                    Tipo = row["DATA_TYPE"],
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
