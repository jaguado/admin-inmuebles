﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminInmuebles.Filters
{
    public class BaseResultFilter : IAsyncActionFilter
    {
        const int defaultLimit = 50;
        public static string[] Operators = new[] { "==", "!=", "<", ">", "<>", "<=", ">=" };
        private static readonly bool _minifyResponse = Environment.GetEnvironmentVariable("minifyResponse") =="true" ? true : false;

        private static WebMarkupMin.Core.CrockfordJsMinifier minifyJs = new WebMarkupMin.Core.CrockfordJsMinifier();
        private void FilterOrderLimitResult<T>(ActionExecutedContext context, HttpRequest request, OkObjectResult resultResponse)
        {
            if (resultResponse != null)
            {
                var result = resultResponse.Value as IEnumerable<T>;
                if (result != null)
                {
                    if (result.Any())
                    {
                        var newObject = result;
                        var controller = context.Controller as Controller;
                        newObject = FilterResult(result, controller.Request);
                        newObject = OrderResult(newObject, controller.Request);
                        newObject = LimitObjectResult(newObject, controller.Request);
                        //FIXME minify dynamic content, problems with int[]
                        if (_minifyResponse)
                        {
                            var json = JsonConvert.SerializeObject(newObject, Startup.jsonSettings);
                            var minified = minifyJs.Minify(json, false);
                            if (!minified.Errors.Any())
                                context.Result = new OkObjectResult(minified.MinifiedContent);
                            else
                                context.Result = new OkObjectResult(newObject);
                        }
                        else
                            context.Result = new OkObjectResult(newObject);
                    }
                }
            }
        }

        public static IEnumerable<T> FilterResult<T>(IEnumerable<T> filteredResult, HttpRequest Request)
        {
            var filters = Request.Query["filters"];
            if (filters.Any())
            {
                var query = BuildQueryFromRequest(filters, out List<object> values);
                filteredResult = filteredResult.AsQueryable().Where(query, values.ToArray());
            }
            return filteredResult.AsQueryable<T>();
        }
        public static IEnumerable<T> OrderResult<T>(IEnumerable<T> filteredResult, HttpRequest Request)
        {
            var order = Request.Query["order"];
            if (order.Any())
                foreach (var o in order)
                    filteredResult = filteredResult.AsQueryable<T>().OrderBy(o);
            return filteredResult;
        }
        internal IEnumerable<T> LimitObjectResult<T>(IEnumerable<T> filteredResult, HttpRequest Request)
        {
            if (int.TryParse(Request.Query["offset"], out int offset))
                filteredResult = filteredResult.Skip(offset);

            if (int.TryParse(Request.Query["limit"], out int limit))
            {
                if (limit == 0)
                    return filteredResult;
                else
                    return filteredResult.Take(limit);
            }
            return filteredResult.Take(defaultLimit);
        }

        private static string BuildQueryFromRequest(Microsoft.Extensions.Primitives.StringValues filters, out List<object> values)
        {
            var query = "";
            values = new List<object>();
            foreach (var filter in filters)
            {
                foreach (var op in Operators)
                {
                    var args = filter.Split(op);
                    if (args.Length > 1)
                    {
                        var field = args[0];
                        var value = args[1];
                        if (query != string.Empty)
                            query += " and ";
                        query += $"{field} {op} @{values.Count}";
                        //check if numeric
                        if (int.TryParse(value, out int newValue))
                            values.Add(newValue);
                        else
                            values.Add(value);
                    }
                }
            }
            return query;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            // do something after the action executes
            var request = resultContext.HttpContext.Request;
            var resultResponse = resultContext.Result as OkObjectResult;
            //TODO identify type!!
            if (resultResponse != null && resultResponse.Value != null)
            {
                switch (resultResponse.Value)
                {
                    case IEnumerable<int> t:
                        FilterOrderLimitResult<int>(resultContext, request, resultResponse);
                        break;
                    case IEnumerable<string> t:
                        FilterOrderLimitResult<string>(resultContext, request, resultResponse);
                        break;
                    case IEnumerable<DateTime> t:
                        FilterOrderLimitResult<DateTime>(resultContext, request, resultResponse);
                        break;
                    case IEnumerable<object> t:
                        FilterOrderLimitResult<object>(resultContext, request, resultResponse);
                        break;
                }
            }
            context.Result = resultContext.Result;
        }
    }
}
