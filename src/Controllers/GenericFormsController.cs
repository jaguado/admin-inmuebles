using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
                        BD = row["TABLE_CATALOG"].StringOrEmpty(),
                        Esquema = row["TABLE_SCHEMA"].StringOrEmpty(),
                        Nombre = row["TABLE_NAME"].StringOrEmpty()
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
                var spColumnas = $"sp_Columns '{tableName}'";
                var tables = await Helpers.Sql.GetData(spColumnas);
                if (tables == null || tables.Tables[0].Rows.Count == 0)
                    return new NotFoundResult();
                var rows = tables.Tables[0].Select().ToList();
                var output = rows.Select(row =>
                {
                    return new Models.Campo
                    {
                        Nombre = row["COLUMN_NAME"].StringOrEmpty(),
                        Tipo = row["TYPE_NAME"].StringOrEmpty(),
                        Opcional = row["NULLABLE"] != null && row["NULLABLE"].StringOrEmpty() == "1",
                        Largo = row["LENGTH"].IntOrDefault(),
                        Precision = row["PRECISION"].IntOrDefault()
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

        [HttpPut("{tableName}")]
        public async Task<IActionResult> UpdateData(string tableName)
        {
            if (!IsAdminAtLeast())
                return new UnauthorizedObjectResult("'Admin' role required");


            if (!Request.HasFormContentType)
            {
                // json
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                var obj = JsonConvert.DeserializeObject(body) as JObject;
                if (obj["_columns"] != null)
                {
                    var columns = JsonConvert.DeserializeObject<Models.Campo[]>(obj["_columns"].ToString());
                    if (columns != null && columns.Any())
                    {
                        //TODO create update query
                        var index = 0;
                        var query = $"UPDATE {tableName} SET ";
                        columns.Where(c => !c.IsIdentity).ToList().ForEach(column =>
                        {
                            if (index > 0)
                                query += ", ";
                            query += $"{column.Nombre} = '{obj[column.Nombre]}'";
                            index++;
                        });
                        index = 0;
                        query += " WHERE ";
                        columns.Where(c => c.IsIdentity).ToList().ForEach(column =>
                        {
                            if (index > 0)
                                query += " and ";
                            query += $"{column.Nombre} = {obj[column.Nombre]}";
                            index++;
                        });
                        //TODO exec query
                        var tables = await Helpers.Sql.GetData(query);
                        return new OkResult();
                    }
                }
            }

            var payload = await Request.ReadFormAsync();
            Console.WriteLine($"UpdateData payload:");
            payload.Keys.ToList().ForEach(key =>
            {
                if (payload.TryGetValue(key, out StringValues value))
                    Console.WriteLine($"{key}: {value}");
                else
                    Console.WriteLine($"{key}:  error getting value");
            });
            throw new NotImplementedException("FormContentType not implemented");
        }
    }
}
