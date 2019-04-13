using AdminInmuebles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdminInmueblesTests
{
    [TestClass]
    public class AppTests
    {
        public static void LoadTestVariables()
        {
            System.Environment.SetEnvironmentVariable("SQL_CONN", "Data Source=50.97.128.140;Database=desoincl_adm_inmueble;Integrated Security=false;User ID=desoincl_inmueble;Password=Carito20.");
        }
    }
}
