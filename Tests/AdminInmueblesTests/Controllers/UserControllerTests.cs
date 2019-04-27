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
        [TestMethod]
        public async Task GetDefautMenu()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.UserController();
            var result = await controller.GetAuthUser("","") as OkObjectResult;
            Assert.IsNotNull(result);
            var data = result.Value as dynamic;
            Assert.IsNotNull(data);

        }
    }
}
