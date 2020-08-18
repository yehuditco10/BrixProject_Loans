using AutoMapper;
using Loan.Api.Middleware;
using Loan.Data;
using Loan.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Loan.Api
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
            services.AddDbContext<LoanContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("LoanConnection")));
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<ILoanRepository, LoanRepository>();
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
                setupAction.SwaggerDoc("LoansApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "BrixProject - Loans",
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
                    "/swagger/LoansApiSpecification/swagger.json",
                    "BrixProject - Loans");
                setupAction.RoutePrefix = "";
            });
        }
    }
}
