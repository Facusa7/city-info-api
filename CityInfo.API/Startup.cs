using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.API
{
    /// <summary>
    /// This class is an entry point for the application.
    /// </summary>
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // What is a service in .NET Core? It's a component that is intended for common consumption in an application: There is framework services, like, Identity,
        // MVC, EF Core Services; and there're also application services, which are application-specific: A component to send mail.
        // and there're others that are built-in like the ApplicationBuilder and a Logger
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            //This will handle the status pages when it's needed. 
            app.UseStatusCodePages();
            /* We add this after the exception handler, so that middleware can potentially catch exceptions before
             handling the request over to MVC and, more importantly, handle exceptions and return the correct response
             when an exception happens in the MVC-related code we'll write.
             At this moment, MVC middleware will handle MVC requests */
            app.UseMvc();
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

        }
    }
}
