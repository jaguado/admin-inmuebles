using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmueblesTests.ActionFilters
{
    [TestClass]
    public class BaseResultFilterUnitTest
    {
        private static ActionContext actionContext = new ActionContext()
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };
        private static AdminInmuebles.Controllers.BaseController controller = new AdminInmuebles.Controllers.CustomerController();
        private static AdminInmuebles.Filters.BaseResultFilter filter = new AdminInmuebles.Filters.BaseResultFilter();
        private static int[]  numbers = new int[] { 1, 3, 4, 9, 7, 6, 2 };
        private static ActionExecutingContext testContext = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), new Dictionary<string, object>(), controller)
        {
            Result = new OkObjectResult(numbers.AsEnumerable())
        };

        [TestMethod]
        public async Task OnActionExecutionWithoutFilters()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(testContext)));
            await filter.OnActionExecutionAsync(testContext, next);
            var result = testContext.Result as OkObjectResult;
            Assert.IsNotNull(result);
            var resultValue = result.Value as IEnumerable<int>;
            Assert.IsNotNull(resultValue);
            var finalArray = resultValue.ToArray();
            Assert.AreEqual(numbers.Length, finalArray.Length);
            for (int i = 0; i < numbers.Length; i++)
            {
                Assert.AreEqual(numbers[i], finalArray[i]);
            }
        }

        [TestMethod]
        public async Task OnActionExecutionWithFilter()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString($"?filter=1==1");
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(testContext)));
            await filter.OnActionExecutionAsync(testContext, next);
            var result = testContext.Result as OkObjectResult;
            Assert.IsNotNull(result);
            var resultValue = result.Value as IEnumerable<int>;
            Assert.IsNotNull(resultValue);
            var finalArray = resultValue.ToArray();
            Assert.AreEqual(numbers.Length, finalArray.Length);
            for (int i = 0; i < numbers.Length; i++)
            {
                Assert.AreEqual(numbers[i], finalArray[i]);
            }
        }

        [TestMethod]
        public async Task OnActionExecutionWithOrder()
        {
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString($"?order=1");
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(testContext)));
            await filter.OnActionExecutionAsync(testContext, next);
            var result = testContext.Result as OkObjectResult;
            Assert.IsNotNull(result);
            //FIXME validate correct order Assert.AreNotEqual(numbers, result.Value);
        }

        [TestMethod]
        public async Task OnActionExecutionWithLimit()
        {
            const int limit = 3;
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString($"?limit={limit}");
            var next = new ActionExecutionDelegate(() => Task.FromResult(CreateActionExecutedContext(testContext)));
            await filter.OnActionExecutionAsync(testContext, next);
            var result = testContext.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(limit, (result.Value as IEnumerable<int>).Count());
        }

        private ActionExecutingContext CreateActionExecutingContext(IFilterMetadata filter)
        {
            return new ActionExecutingContext(
                actionContext,
                new IFilterMetadata[] { filter, },
                new Dictionary<string, object>(),
                controller: new object());
        }

        private ActionExecutedContext CreateActionExecutedContext(ActionExecutingContext context)
        {
            return new ActionExecutedContext(context, context.Filters, context.Controller)
            {
                Result = context.Result
            };
        }
    }
}
