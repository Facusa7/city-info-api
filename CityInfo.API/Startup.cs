using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    /// <summary>
    /// This class is an entry point for the application.
    /// </summary>
    public class Startup
    {
        //Static configuration variable so we can use it application-wide
        public static IConfiguration Configuration { get; private set; }
        /// <summary>
        /// In ASP.NET core 2, the CreateDefaultBuilder call on the WebHost in the Program class already sets up the configuration files.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // What is a service in .NET Core? It's a component that is intended for common consumption in an application: There is framework services, like, Identity,
        // MVC, EF Core Services; and there're also application services, which are application-specific: A component to send mail.
        // and there're others that are built-in like the ApplicationBuilder and a Logger
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                //AddMvcOptions allows us to configure the supported formatters for our API. In this case, we add the XML output formatter.
                //So, when a request with Accept:application/xml header is done, we can return a propper response. 
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));
            /*
             * The AddJsonOptions it's used when we want to set up the way we serialize the properties on our classes.
             */
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        //From this moment on, JSON.NET will simply take the property names as they are defined on our class. 
            //        castedResolver.NamingStrategy = null; 
            //    }

            //});

            /* This type of services are created each time they are requested. This lifetime works best for lightweight stateless services. 
             From this moment, an instance can be injected */

#if DEBUG //These are compiler directives
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(option => option.UseSqlServer(connectionString));
            //Service that would be created once per request. 
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            //ILoggerFactory is a built-in service which allows us to create different type of logging in our project. 
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            //for other providers, you should add it like this: loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            cityInfoContext.EnsureSeedDataForContext();
            //AutoMapper is convention based, it will map properties with the same name in the source and destination.
            //By default, it will ignore NullReference Exceptions from source to target.
            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
                    cfg.CreateMap<Entities.City, Models.CityDto>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointsOfInterestDto>();
                    cfg.CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
                    cfg.CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
                });

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
