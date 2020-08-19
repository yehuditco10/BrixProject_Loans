using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rules.Api.Middleware;
using Rules.Data;
using Rules.Services;
using Rules.Services.Models;

namespace Rules.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<RulesContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("RuleConnection")),
                   ServiceLifetime.Scoped);
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<IRuleRepository, RuleRepository>();
            services.AddSingleton<RulesTranslate>(new RulesTranslate()
            {
                Parameters = new Dictionary<string, string>()
                {
                    {"גיל", "Age"},
                    { "יתרה", "Balance" },
                    { "עיר", "City" }

        },
                Signs = new Dictionary<string, string>()
                {
                    {"גדול מ", ">" },
                    { "גדולה מ", ">"},
                    {"קטן מ", "<"},
                    {"קטנה מ", "<"},
                    { "שווה ל", "="}
        }
            });
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("RulesApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "BrixProject - Rules",
                        Version = "1",
                        Description = "Brix Final Project",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Name = "Yehudit Cohen",
                            Email = "cyehudit10@gmail.com"
                        }
                    });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/RulesApiSpecification/swagger.json",
                    "BrixProject - Rules");
                setupAction.RoutePrefix = "";
            });
        }
    }
}
