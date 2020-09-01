using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExploreCalifornia
{
    public class Startup
    {
        // # # #
        private readonly IConfiguration configuration;

        // # # # function used for letting the dev access the configuration object
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggle>(x => new FeatureToggle()
            {
                DeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions")
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FeatureToggle features)
        {
            app.UseExceptionHandler("/error.html");
            // # # # 1. properties -> debug -> if (ENVIRONMENT = Development) => developer error page ( useful for dev )
            //                            else custom error page seen by user ( in this case error.html )
            // env.IsDevelopment() -> code used for first approach
            // 2. By using the configuration object ( creating Startup function ) the dev has more "freedom".
            // configuration.GetValue<bool>("EnableDeveloperExceptions") -> code used for second approach
            // dev can create env variable named EnableDeveloperExceptions... if there's no value in the config object it returns null
            // 3. The dev can update the json configuration object and then access the new setting in the json here
            // configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions") -> code used for third approach ( : is used to access setting from json object, not . )
            // good practice, use a default json object for default settings and then create other json settings object...
            // 4. creating a instance of FeatureToggle object trough addTransient function in ConfigureServices function from which the json
            // setting is read and then used for this.
            // current code in the if -> code used for last approach
            if (features.DeveloperExceptions) 
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseRouting();

            app.Use(async (context, next) =>
            { 
                if (context.Request.Path.Value.Contains("invalid"))
                throw new Exception("ERROR!");

            await next();
            });


            // # # # This function is used for routing any request to the static html files from the project
            app.UseFileServer();
        }
    }
}

/*
 * Other comments about the program:
 * 1. A configuration json file is created implicitly besides the default one 
 * the app makes the choice by checking the env variable found in the app properties
 * appsettings.json and appsettings.Development.json
 * 2. Regarding "COnfigureServices" function above, services.function_name -> methods that allows configuration of dependency logic
 * addTransient -> shortest lifespan, create a new instance everytime that is used ( by other methods.. )
 * addScoped -> permits to share state between different components trough the same request
 * addSingleton -> only one for every request or application lifetime ( for all users )
 */
