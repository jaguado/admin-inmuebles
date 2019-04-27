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
    public class UserController : BaseController
    {
        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            return new OkObjectResult(await MongoDB.FromMongoDB<Models.User>());
        }
        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] Models.User user)
        {
            return await user.ToMongoDB<Models.User>();
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] Models.User user)
        {
            return await user.ToMongoDB<Models.User>();
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetAuthUser(string forUser, string userInfo)
        {
            //TODO get real condos, menus and roles info
            return new OkObjectResult(new
            {
                User = JsonConvert.DeserializeObject(userInfo),
                Condos = new List<object>(5),
                Roles = new List<object>(5),
                Menus = await GetDefaultMenu()
            });
        }

        private async Task<List<Models.Section>> GetDefaultMenu()
        {
            return new List<Models.Section>
            {
                new Models.Section
                {
                    Name="Opciones",
                    Menus = new List<Models.Menu>{await GetMantenedores() }
                }
            };
        }

        private async Task<Models.Menu> GetMantenedores()
        {
            var result = await new GenericFormsController().GetTypesTables() as OkObjectResult;
            var tablas = result.Value as IList<Models.Tabla>;
            var menu = new Models.Menu
            {
                Name = "Mantenedores",
                IconClass = "ti-settings",
                Options = tablas!=null ? tablas.Select(tabla =>
                {
                    return new Models.Menu.Option
                    {
                        Title = tabla.Nombre,
                        Href = "GenericFormTest.html?id=" + tabla.Nombre
                    };
                }).ToList(): new List<Models.Menu.Option>()
            };
            return menu;
        }
    }
}
