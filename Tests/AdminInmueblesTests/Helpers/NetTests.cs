using AdminInmuebles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdminInmueblesTests
{
    [TestClass]
    public class NetTests
    {
        [TestMethod(), Description("Check internet access")]
        public void Ping()
        {
            var result = AdminInmuebles.Helpers.Net.Ping("www.google.cl").Result;
            Assert.IsNotNull(result);
        }
    }
}
