using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Controllers
{
    /// <summary>
    /// This controller allows to manage all the types tables so only admin or superior roles have access
    /// </summary>
    [Route("v1/[controller]")]
    public class GenericFormsController : BaseController
    {
        [HttpGet()]
        [Produces(typeof(IList<Models.Tabla>))]
        public async Task<IActionResult> GetTypesTables()
        {
            try
            {
                if (!IsAdminAtLeast())
                    return new UnauthorizedObjectResult("'Admin' role required");
                const string queryMantenedores = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME like 'TIPO_%'";
                var tables = await Helpers.Sql.GetData(queryMantenedores);  
                if (tables == null || tables.Tables[0].Rows.Count == 0)
                    return null;
                var rows = tables.Tables[0].Select().ToList();
                var output = rows.Select(row =>
                {
                    return new Models.Tabla
                    {
                        BD = row["TABLE_CATALOG"].ToString(),
                        Esquema = row["TABLE_SCHEMA"].ToString(),
                        Nombre = row["TABLE_NAME"].ToString()
                    };
                }).ToList();
                return new OkObjectResult(output);
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                await Console.Error.WriteLineAsync($"{ex.Message} / {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{tableName}")]
        [Produces(typeof(IEnumerable<Models.Campo>))]
        public async Task<IActionResult> GetTableDetail(string tableName)
        {
            try
            {
                if (!IsAdminAtLeast())
                    return new UnauthorizedObjectResult("'Admin' role required");
                var queryTabla = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = '{tableName}'";
                var tables = await Helpers.Sql.GetData(queryTabla);
                if (tables == null || tables.Tables[0].Rows.Count == 0)
                    return new NotFoundResult();
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
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                await Console.Error.WriteLineAsync($"{ex.Message} / {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{tableName}/data")]
        public async Task<IActionResult> GetTableData(string tableName, int maxRowCount = 50)
        {
            try
            {
                if (!IsAdminAtLeast())
                    return new UnauthorizedObjectResult("'Admin' role required");
                var filter = maxRowCount > 0 ? $"TOP {maxRowCount}" : "";
                var queryTabla = $"SELECT {filter} * FROM {tableName}";
                var tables = await Helpers.Sql.GetData(queryTabla);
                if (tables == null || tables.Tables[0].Rows.Count == 0)
                    return new NotFoundResult();
                return new OkObjectResult(tables.Tables[0]);
            }
            catch (Exception ex)
            {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
                await Console.Error.WriteLineAsync($"{ex.Message} / {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{tableName}")]
        public async Task<IActionResult> AddData(string tableName)
        {
            if (!IsAdminAtLeast())
                return new UnauthorizedObjectResult("'Admin' role required");

            if (!Request.HasFormContentType)
                return StatusCode(500, "Request has not FormContentType");

            var payload = await Request.ReadFormAsync();
            Console.WriteLine($"AddData payload:");
            payload.Keys.ToList().ForEach(key =>
            {
                if(payload.TryGetValue(key, out StringValues value))
                    Console.WriteLine($"{key}: {value}");
                else
                    Console.WriteLine($"{key}:  error getting value");
            });
            return new OkObjectResult(payload.Keys);
        }

        [HttpPut("{nombre}")]
        public async Task<IActionResult> UpdateData(string nombre)
        {
            if (!IsAdminAtLeast())
                return new UnauthorizedObjectResult("'Admin' role required");

            if (!Request.HasFormContentType)
                return StatusCode(500, "Request has not FormContentType");

            var payload = await Request.ReadFormAsync();
            Console.WriteLine($"UpdateData payload:");
            payload.Keys.ToList().ForEach(key =>
            {
                if (payload.TryGetValue(key, out StringValues value))
                    Console.WriteLine($"{key}: {value}");
                else
                    Console.WriteLine($"{key}:  error getting value");
            });
            return new OkObjectResult(payload.Keys);
        }
    }
}
