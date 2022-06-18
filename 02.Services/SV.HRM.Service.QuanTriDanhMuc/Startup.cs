using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SV.HRM.Logging.Extensions;
using SV.HRM.Logging.NLogCustom;
using SV.HRM.Service.QuanTriDanhMuc.Ioc;

namespace SV.HRM.Service.QuanTriDanhMuc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region UserNLog
            Console.WriteLine($"Welcome to SV.HRM------------->{DateTime.Now}");

            var configFilePath = configuration.GetSection("Logging:Providers:NLog:ConfigFilePath").Get<string>();
            LoggingBuilderExten.UseNLog(configFilePath, configuration.GetSection("Logging:KafkaTaget").Get<string>(), LogSourceTypeEnums.HRM_Service_QuanTriDanhMuc);

            #endregion UserNLog
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterIoCs(Configuration);
            services.AddControllers().AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ContractResolver = new DefaultContractResolver(); //Return Uppercase
                opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddLogging();


            //add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v3",
                    Title = "API Service QuanTriDanhMuc",
                    Description = "",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
