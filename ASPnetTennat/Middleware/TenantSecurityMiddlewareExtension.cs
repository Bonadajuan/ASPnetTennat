using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat.Middleware
{
    // El UseTenantmétodo de extensión agrega el middleware 
    // a la canalización de procesamiento de solicitudes
    public static class TenantSecurityMiddlewareExtension
    {
        public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}
