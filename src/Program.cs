using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using AdminInmuebles.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace AdminInmuebles
{
    public class Program
    {
        public static bool isDev = false;
        public static void Main(string[] args)
        {
            var url = "http://*:" + Environment.GetEnvironmentVariable("PORT") ?? throw new ApplicationException("'PORT' variable must be defined");
            NewRelic.Api.Agent.NewRelic.StartAgent();
            Console.WriteLine("Starting web server on " + url);
            BuildWebHost(args, url).Run();
        }

        public static IWebHost BuildWebHost(string[] args, string url) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();
    }
}
