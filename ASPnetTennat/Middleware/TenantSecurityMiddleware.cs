using ASPnetTennat.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPnetTennat.Middleware
{
    // Se usa el middleware para leer la clave API del encabezado de la solicitud y se usa para recuperarla Tenant ID de la base de datos. 
    // El middleware es un componente que puede manejar solicitudes y respuestas. 
    // Por último, lo Tenant ID recuperado se almacena en la sesion
    // la clave API se pasa en el encabezado de la solicitud. Esta clave se utiliza para recuperar el Tenant ID. 
    // El TenantId luego se almacena en la sesión
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            string tenantIdentifier = context.Session.GetString("TenantId");
            if (string.IsNullOrEmpty(tenantIdentifier))
            {
                var apiKey = context.Request.Headers["X -Api-Key"].FirstOrDefault();
                if (string.IsNullOrEmpty(apiKey))
                {
                    return;
                }

                Guid apiKeyGuid;
                if (!Guid.TryParse(apiKey, out apiKeyGuid))
                {
                    return;
                }

                TenantRepository _tenentRepository = new TenantRepository(configuration, httpContextAccessor);
                string tenantId = await _tenentRepository.GetTenantId(apiKeyGuid);
                context.Session.SetString("TenantId", tenantId);
            }
            await next.Invoke(context);
        }
    }

}
