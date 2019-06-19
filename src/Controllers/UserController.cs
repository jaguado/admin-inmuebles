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
        [Produces(typeof(Models.AuthenticatedUser))]
        public async Task<IActionResult> GetAuthUser(string forUser, string userInfo)
        {
            //TODO get real condos, menus and roles info
            return new OkObjectResult(new Models.AuthenticatedUser
            {
                User = new Models.User {
                    Info = userInfo!=null ? JsonConvert.DeserializeObject(userInfo): null
                },
                Menu = await GetDefaultMenu()
            });
        }

        private async Task<List<Models.MenuSection>> GetDefaultMenu()
        {
            return new List<Models.MenuSection>
            {
                new Models.MenuSection
                {
                    Section="Personal",
                    Menus = new List<Models.Menu>
                    {
                        new Models.Menu
                        {
                            Name="Dashboard",
                            IconClass="mdi mdi-gauge",
                            Options = new List<Models.Menu.Option>
                            {
                                new Models.Menu.Option
                                {
                                    Title="Dashboard 1",
                                    Href="index2.html",
                                    BreadcrumbLabel="Personal / Dashboard"
                                },
                                new Models.Menu.Option
                                {
                                    Title="Test Page",
                                    Href="/Views/testPage.html"
                                }
                            }
                        }
                    }
                },
                new Models.MenuSection
                {
                    Section="Opciones",
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
                    var nombre = tabla.Nombre.Replace("TIPO_", "");
                    return new Models.Menu.Option
                    {
                        Title = nombre,
                        Href = "GenericFormTest.html?id=" + nombre
                    };
                }).ToList(): new List<Models.Menu.Option>()
            };
            return menu;
        }
    }
}
