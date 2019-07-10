using AdminInmuebles.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminInmueblesTests
{
    [TestClass]
    public class GenericFormsTests
    {
        internal string _validJwt = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBCODAxMUJFN0U2OEY0QzA4MkEwODA3MTkzQjU2N0EyQzlBNjJBNUYiLCJ4NXQiOiJDNEFSdm41bzlNQ0NvSUJ4azdWbm9zbW1LbDgiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJBZG1Jbm11ZWJsZXMuRGV2ZWxvcG1lbnQiLCJlbWFpbCI6ImpvcmdlQGphbXRlY2guY2wiLCJuYW1lIjoiam9yZ2VAamFtdGVjaC5jbCIsImRhdGEiOiJ7XCJSdXRcIjowLFwiTWFpbFwiOlwiam9yZ2VAamFtdGVjaC5jbFwiLFwiTm9tYnJlXCI6XCJqb3JnZUBqYW10ZWNoLmNsXCIsXCJUaXBvXCI6MixcIkVzdGFkb1wiOjEsXCJJY29ub1wiOlwiXCIsXCJQYXNzd29yZFwiOm51bGwsXCJDb25kb3NcIjpbe1wiSWRcIjoxLFwiUnV0XCI6MCxcIlJhem9uU29jaWFsXCI6XCJDT05ET01JTklPIEFOVEFSRVMgREUgTEEgTFVaXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MSxcIlJvbGVzXCI6WzFdLFwiQ3JlYXRlZEJ5XCI6bnVsbCxcIkNyZWF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXBkYXRlZEJ5XCI6bnVsbCxcIlVwZGF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiQXR0cmlidXRlc1wiOntcIktleVwiOm51bGwsXCJWYWx1ZVwiOm51bGx9fSx7XCJJZFwiOjIsXCJSdXRcIjowLFwiUmF6b25Tb2NpYWxcIjpcIkVESUZJQ0lPIERPTiBRVUlKT1RFXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MSxcIlJvbGVzXCI6WzJdLFwiQ3JlYXRlZEJ5XCI6bnVsbCxcIkNyZWF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXBkYXRlZEJ5XCI6bnVsbCxcIlVwZGF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiQXR0cmlidXRlc1wiOntcIktleVwiOm51bGwsXCJWYWx1ZVwiOm51bGx9fSx7XCJJZFwiOjMsXCJSdXRcIjowLFwiUmF6b25Tb2NpYWxcIjpcIkNPTkRPTUlOSU8gQkxPUVVFQURPXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MixcIlJvbGVzXCI6WzcsMV0sXCJDcmVhdGVkQnlcIjpudWxsLFwiQ3JlYXRlZE9uXCI6XCIwMDAxLTAxLTAxVDAwOjAwOjAwXCIsXCJVcGRhdGVkQnlcIjpudWxsLFwiVXBkYXRlZE9uXCI6XCIwMDAxLTAxLTAxVDAwOjAwOjAwXCIsXCJBdHRyaWJ1dGVzXCI6e1wiS2V5XCI6bnVsbCxcIlZhbHVlXCI6bnVsbH19XSxcIkNyZWF0ZWRCeVwiOm51bGwsXCJDcmVhdGVkT25cIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIlVwZGF0ZWRCeVwiOm51bGwsXCJVcGRhdGVkT25cIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIkF0dHJpYnV0ZXNcIjp7XCJLZXlcIjpudWxsLFwiVmFsdWVcIjpudWxsfX0iLCJyb2xlcyI6IltcIkdvZFwiLFwiVXNlclwiLFwiQWRtaW5cIl0iLCJleHAiOjc1NjI2NTQxODF9.k8Ew5DfYbkqvsrwytf7rke5A7T0QGrU5JyovGz4h2Fn9h5rmHQRcIjbbV2FIeXR6ybPdHoSBI-G0r5tkmOPbEevmObJMcIZVeqvM467FCg3Ot2dDejbTTR3JjGddVBClEahbLmwtkwAOQbkujONBhV0aH5xe3ER81AkvAZM94il0_luB-9o-kGT-8qLxORMCY6YF1OVYOhvoCSCpIIQE_yPdbP4l1qGHHABp2liVhgcy_XWkrLRoerPTnpsl1i_3FhgYjYBtXAhbYSc7wflspKdd7mIphXJFbUZXLjQqOU8uRsIaMkDHlqFTUfsdAbJzqimPJMdKzEguq2osbsjwxA";
        internal string _validJwtWithoutAdmin = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBCODAxMUJFN0U2OEY0QzA4MkEwODA3MTkzQjU2N0EyQzlBNjJBNUYiLCJ4NXQiOiJDNEFSdm41bzlNQ0NvSUJ4azdWbm9zbW1LbDgiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJBZG1Jbm11ZWJsZXMuRGV2ZWxvcG1lbnQiLCJlbWFpbCI6ImpvcmdlQGphbXRlY2guY2wiLCJuYW1lIjoiam9yZ2VAamFtdGVjaC5jbCIsImRhdGEiOiJ7XCJSdXRcIjowLFwiTWFpbFwiOlwiam9yZ2VAamFtdGVjaC5jbFwiLFwiTm9tYnJlXCI6XCJqb3JnZUBqYW10ZWNoLmNsXCIsXCJUaXBvXCI6MixcIkVzdGFkb1wiOjEsXCJJY29ub1wiOlwiXCIsXCJQYXNzd29yZFwiOm51bGwsXCJDb25kb3NcIjpbe1wiSWRcIjoxLFwiUnV0XCI6MCxcIlJhem9uU29jaWFsXCI6XCJDT05ET01JTklPIEFOVEFSRVMgREUgTEEgTFVaXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MSxcIlJvbGVzXCI6WzJdLFwiQ3JlYXRlZEJ5XCI6bnVsbCxcIkNyZWF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXBkYXRlZEJ5XCI6bnVsbCxcIlVwZGF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiQXR0cmlidXRlc1wiOntcIktleVwiOm51bGwsXCJWYWx1ZVwiOm51bGx9fSx7XCJJZFwiOjIsXCJSdXRcIjowLFwiUmF6b25Tb2NpYWxcIjpcIkVESUZJQ0lPIERPTiBRVUlKT1RFXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MSxcIlJvbGVzXCI6WzJdLFwiQ3JlYXRlZEJ5XCI6bnVsbCxcIkNyZWF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXBkYXRlZEJ5XCI6bnVsbCxcIlVwZGF0ZWRPblwiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiQXR0cmlidXRlc1wiOntcIktleVwiOm51bGwsXCJWYWx1ZVwiOm51bGx9fSx7XCJJZFwiOjMsXCJSdXRcIjowLFwiUmF6b25Tb2NpYWxcIjpcIkNPTkRPTUlOSU8gQkxPUVVFQURPXCIsXCJUaXBvXCI6MSxcIlZpZ2VuY2lhXCI6MixcIlJvbGVzXCI6WzIsMl0sXCJDcmVhdGVkQnlcIjpudWxsLFwiQ3JlYXRlZE9uXCI6XCIwMDAxLTAxLTAxVDAwOjAwOjAwXCIsXCJVcGRhdGVkQnlcIjpudWxsLFwiVXBkYXRlZE9uXCI6XCIwMDAxLTAxLTAxVDAwOjAwOjAwXCIsXCJBdHRyaWJ1dGVzXCI6e1wiS2V5XCI6bnVsbCxcIlZhbHVlXCI6bnVsbH19XSxcIkNyZWF0ZWRCeVwiOm51bGwsXCJDcmVhdGVkT25cIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIlVwZGF0ZWRCeVwiOm51bGwsXCJVcGRhdGVkT25cIjpcIjAwMDEtMDEtMDFUMDA6MDA6MDBcIixcIkF0dHJpYnV0ZXNcIjp7XCJLZXlcIjpudWxsLFwiVmFsdWVcIjpudWxsfX0iLCJyb2xlcyI6IltcIlVzZXJcIl0iLCJleHAiOjc1NjI2NTUyNjN9.V08FfHzqDi2HYKT5BixsCatki7hhX2sx07naw-LnXnXpfBBrsd3ik3B0SMsujNv7ZPq0VczuvxD84mWrUP6HKAj9jNZe3ZXXxhHdUWWkb88lXpYcowA1mZXZ132OUADBqJQ0Z6SFlGEHX2SgUWewBATCd0LzUlFhcr1F176HWI7dvhoR-zInFHdZ1I_7KhltErvnhqwta0_NU8gub8jCVSzdMfhp8lo2lVgO8MUBkvsZE9dhe7g74kdF2Hb6U1SZV6e028yFYspfiLWoxBxpNzkI4LAqV2IbW8cXeTLxm4JUmzH9tN6SKdkWbVu4SKCHTkEqL92Qlutf7DIphTCsdQ";
        const string _testTableName = "TIPO_USUARIO_ROL";

        [TestMethod]
        public async Task GetTables()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwt.ToJwt(true, true)
            };
            var result = await controller.GetTypesTables() as OkObjectResult;
            Assert.IsNotNull(result);
            var value = result.Value as IEnumerable<AdminInmuebles.Models.Tabla>;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public async Task GetTableDetail()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwt.ToJwt(true, true)
            };
            var result = await controller.GetTableDetail(_testTableName) as OkObjectResult;
            Assert.IsNotNull(result);
            var value = result.Value as IEnumerable<AdminInmuebles.Models.Campo>;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public async Task GetTablesWithoutValidAuth()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwtWithoutAdmin.ToJwt(true,true)
            };
            var result = await controller.GetTypesTables() as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTableDetailWithoutValidAuth()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwtWithoutAdmin.ToJwt(true,true)
            };
            var result = await controller.GetTableDetail(_testTableName) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTableData()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwt.ToJwt(true, true)
            };
            var result = await controller.GetTableData(_testTableName) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public async Task GetTableDataWithoutValidAuth()
        {
            AppTests.LoadTestVariables();
            var controller = new AdminInmuebles.Controllers.GenericFormsController()
            {
                AuthenticatedToken = _validJwtWithoutAdmin.ToJwt(true, true)
            };
            var result = await controller.GetTableData(_testTableName) as UnauthorizedObjectResult;
            Assert.IsNotNull(result);
        }
    }
}
