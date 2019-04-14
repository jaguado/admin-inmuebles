using AdminInmuebles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmueblesTests
{
    [TestClass]
    public class GenericFormsTests
    {
        [TestMethod]
        public async Task GetTables()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController();
            var result = await controller.GetTypesTables() as OkObjectResult;
            Assert.IsNotNull(result);
            var value = result.Value;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public async Task GetTableDetail()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController();
            var result = await controller.GetTableDetail("TIPO_CARGO_COMITE") as OkObjectResult;
            Assert.IsNotNull(result);
            var value = result.Value;
            Assert.IsNotNull(value);
        }
    }
}
