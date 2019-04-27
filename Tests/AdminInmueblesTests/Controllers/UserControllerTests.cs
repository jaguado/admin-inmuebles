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
        public async Task BuildDefaultMenu()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.UserController();
            var result = await controller.GetDefaultMenu();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}
