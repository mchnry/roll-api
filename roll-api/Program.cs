using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using roll_api.Infrastructure;

namespace roll_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var x = Directory.GetCurrentDirectory();

            return WebHost.CreateDefaultBuilder(args)

                //.UseKestrel((o) => o.AddServerHeader = false)
                .ConfigureKestrel(o => o.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.ConfigureAppConfiguration((ctx, b) =>
                //{
                //    b.SetBasePath(ctx.HostingEnvironment.ContentRootPath);
                //    b.AddJsonFile("appsettings.json", true, true)
                //    .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", true, true);
                //    b.Build();
                //})
                .UseIISIntegration()
                .UseStartup<Startup>();
        }
    }
}
