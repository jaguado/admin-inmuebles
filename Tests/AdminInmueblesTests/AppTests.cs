using AdminInmuebles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace AdminInmueblesTests
{
    [TestClass]
    public class AppTests
    {
        public static void LoadTestVariables()
        {
            Environment.SetEnvironmentVariable("SQL_CONN", "Data Source=50.97.128.140;Database=desoincl_adm_inmueble;Integrated Security=false;User ID=desoincl_inmueble;Password=Carito20.");
            Environment.SetEnvironmentVariable("disableJwtCheck", "true");
        }

        [TestMethod]
        public async Task TestSwagger()
        {
            var webHostBuilder =
                  new WebHostBuilder()
                        //.UseEnvironment("development") // You can set the environment you want (development, staging, production)
                        .UseStartup<Startup>(); // Startup class of your web app project

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.GetAsync("/swagger/v1/swagger.json");
                Assert.IsTrue(result.IsSuccessStatusCode);
                var root = JObject.Parse(await result.Content.ReadAsStringAsync());
                Assert.IsNotNull(root);
            }
        }

        [TestMethod]
        public async Task TestSwaggerUI()
        {
            var webHostBuilder =
                  new WebHostBuilder()
                        //.UseEnvironment("development") // You can set the environment you want (development, staging, production)
                        .UseStartup<Startup>(); // Startup class of your web app project

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                var result = await client.GetAsync("/swagger/index.html");
                Assert.IsTrue(result.IsSuccessStatusCode);
            }
        }
    }
}
