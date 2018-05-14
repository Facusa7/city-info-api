using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CityInfo.API
{
    public class Program
    {
        /// <summary>
        /// This method is the responsible for configuring and running the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        /// <summary>
        /// Web Host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            /* In this case, we'll be running a Web Application, so we need to host it.
             Because of that, an instance of WebHostBuilder is initialized.
             As we are hosting a web application, we need a web server. ASP.NET Core is completely decoupled from the web server enviroment that host the application.
             It actually ships with two different Http servers: WebListener, which is Windows-only web server and Kestrel a cross-platform web server. Kestrel is the default.

            */
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                //This specifies the content root directory used by the web host
                //The content root is the base path to any content used by the app, such as views and its web content.
                //Content root <> Web root 
                .UseContentRoot(Directory.GetCurrentDirectory())
                /* This line signifies that IIS express functions as a reverse proxy server for Kestrel. If you intentd to deploy
                 your application on a Windows Server, you should run IIS as a reverse proxy server that manages and proxies
                 requests to Kestrel
                */
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
