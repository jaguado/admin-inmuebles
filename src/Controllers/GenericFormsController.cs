using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            const string queryMantenedores = "SELECT * FROM information_schema.tables WHERE TABLE_NAME like 'TIPO_%'";
            var allTables = await Helpers.Sql.GetData(queryMantenedores);
            return new OkObjectResult(allTables);
        }

        [HttpGet("{tableName}")]
        public async Task<IActionResult> GetTableDetail(string tableName)
        {
            var queryTabla = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = '{tableName}'";
            var table = await Helpers.Sql.GetData(queryTabla);
            return new OkObjectResult(table);
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
