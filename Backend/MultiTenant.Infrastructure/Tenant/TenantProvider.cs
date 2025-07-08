
using Microsoft.Extensions.Configuration;

namespace MultiTenant.Infrastructure.Tenant
{
    public interface ITenantProvider
    {
        string? TenantId { get; }
        void SetTenant(string tenantId);
        string GetConnectionString();
    }

    public class TenantProvider : ITenantProvider
    {
        private string? _tenantId;
        private readonly IConfiguration _config;

        public TenantProvider(IConfiguration config) => _config = config;

        public string? TenantId => _tenantId;

        public void SetTenant(string tenantId) => _tenantId = tenantId;

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_tenantId))
                throw new InvalidOperationException("Tenant ID is not set");

            var conn = _config.GetConnectionString(_tenantId);
            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException("Tenant connection string not found");

            return conn;
        }
    }
}
