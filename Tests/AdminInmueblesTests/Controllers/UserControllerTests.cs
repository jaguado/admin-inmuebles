using AdminInmuebles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmueblesTests
{
    [TestClass]
    public class UserControllerTests
    {
        const string _loggedUserId = "105560751972558300957";
        const string _loggedUser = "";

        [TestMethod]
        public async Task CheckUser()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.UserController();
            var result = await controller.GetAuthUser(_loggedUserId, _loggedUser) as OkObjectResult;
            Assert.IsNotNull(result);
            var data = result.Value as AdminInmuebles.Models.AuthenticatedUser;
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.User);
            //TODO Assert.IsNotNull(data.User.Info);
        }
        [TestMethod]
        public async Task GetDefautMenu()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.UserController();
            var result = await controller.GetAuthUser(_loggedUserId, _loggedUser) as OkObjectResult;
            Assert.IsNotNull(result);
            var data = result.Value as AdminInmuebles.Models.AuthenticatedUser;
            Assert.IsNotNull(data);
            Assert.IsNotNull(data.Menu);
        }
    }
}
