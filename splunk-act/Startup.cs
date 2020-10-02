using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.HttpsPolicy;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

using Microsoft.Extensions.Logging;

using Serilog;

using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;

namespace splunk_act

{

    public class Startup

    {

        public Startup(IConfiguration configuration)

        {

            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)

        {

            Log.Logger = new LoggerConfiguration()

            .ReadFrom.Configuration(Configuration)

            .Enrich.FromLogContext()

            .WriteTo.EventCollector("http://localhost:8088", "9fdd81b4-3549-44f9-ac4e-85cb7fdad29e")

            .WriteTo.Console(new ElasticsearchJsonFormatter())

            .CreateLogger();

            services.AddSingleton(Log.Logger);
            services.AddAuthorization();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

        {

            if (env.IsDevelopment())

            {

                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>

            {

                endpoints.MapControllers();

            });

        }

    }

}