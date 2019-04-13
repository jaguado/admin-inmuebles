using AdminInmuebles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdminInmueblesTests
{
    [TestClass]
    public class NetTests
    {
        [TestMethod(), Description("Check internet access")]
        public void CheckInternetAccess()
        {
            var result = AdminInmuebles.Helpers.Net.Curl("http://www.google.cl", "GET", "", "").Result;
            Assert.IsNotNull(result);
        }
    }
}
