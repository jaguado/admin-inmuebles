using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminInmuebles.Controllers
{
    public class userViewController : Controller
    {
        public Models.User GetUser(string user)
        {
            return new Models.User
            {
                Address = new Models.Address(),
                Username = "usuario",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Person = new Models.Person() {
                    FirstName = "Primer Nombre",
                    LastName = "Segundo Nombre",
                    Nickname = "usuario",
                    Gender = new Models.Gender() }
            };
        }
    }
}