using ASPnetTennat.Middleware;
using ASPnetTennat.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat
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
            services.AddControllersWithViews();

            // Las clases repository se agregan como servicios de ámbito para que se puedan usar en los 
            // controladores mediante la inyección de dependencia. 
            // HttpContextAccessor agrega una instancia de la clase como servicio singleton. Esto es 
            // necesario porque usará HttpContext en una clase sin controlador: es decir, dentro de 
            // la clase middleware nombrada en TenantSecurityMiddleware
            services.AddScoped<TenantRepository, TenantRepository>();
            services.AddScoped<CustomerRepository, CustomerRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Se registra el middleware en el método Configure de la clase Startup 
            // Esto llama al metodo declarado en el Middlerware TenantSecurityMiddlewareExtension quien a su vez llama a TenantSecurityMiddleware
            app.UseTenant();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
