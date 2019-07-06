using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parking.Api.Commands.Handlers;
using Parking.Api.Models;
using Parking.Api.Queries.Handlers;
using Parking.Api.Services;
using Swashbuckle.AspNetCore.Swagger;
using AuthenticationService = Parking.Api.Services.AuthenticationService;

namespace Parking.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Parking API", Version = "v1" });
            });

            services.AddDbContextPool<DbContext, ParkingContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                );
            });

            services.AddScoped<AuthenticationService>();

            services.AddScoped<ParkingCommandHandler>();
            services.AddScoped<ParkingQueryHandler>();

            services.AddScoped<CommandStoreService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
