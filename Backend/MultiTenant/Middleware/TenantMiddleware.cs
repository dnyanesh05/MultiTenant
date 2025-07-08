
using MultiTenant.Infrastructure.Tenant;

namespace MultiTenant.Api.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var tenantId = context.User.FindFirst("tenantId")?.Value;

            if (!string.IsNullOrEmpty(tenantId))
            {
                tenantProvider.SetTenant(tenantId);
            }

            await _next(context);
        }
    }
}
