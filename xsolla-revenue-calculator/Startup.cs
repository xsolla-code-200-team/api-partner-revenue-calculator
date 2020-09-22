using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.Middlewares;
using xsolla_revenue_calculator.Services;
using xsolla_revenue_calculator.Services.MQConnectionService;
using xsolla_revenue_calculator.Utilities;
using ILogger = DnsClient.Internal.ILogger;

namespace xsolla_revenue_calculator
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
            services.AddControllers();
            services.AddSwaggerGen(c => 
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Xsolla Revenue Calculator API",
                        Description = "First sprint version"
                    });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            ); 
            services.Configure<MongoDbConfiguration>(Configuration.GetSection(nameof(MongoDbConfiguration)));
            services.AddSingleton<IMongoDbConfiguration>(sp =>
                sp.GetRequiredService<IOptions<MongoDbConfiguration>>().Value);
            
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection(nameof(RabbitMqConfiguration)));
            services.AddSingleton<IRabbitMqConfiguration>(sp =>
                sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value);
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddSingleton<IMqConnectionService, MqConnectionService>();
            services.AddSingleton<IDatabaseAccessService, MongoDatabaseAccessService>();
            services.AddScoped<IRevenueForecastService, RevenueForecastService>();
            services.AddScoped<IModelMessagingService, ModelMessagingService>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandlingMiddleware().Invoke
            });
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Xsolla Revenue Calculator API");
                c.RoutePrefix = string.Empty;

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}