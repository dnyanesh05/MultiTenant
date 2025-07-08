using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Interfaces;
using MultiTenant.Domain.Entities;
using MultiTenant.Infrastructure.Data;
using MultiTenant.Infrastructure.Tenant;

namespace MultiTenant.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ITenantProvider _tenantProvider;

        public UserRepository(AppDbContext context, ITenantProvider tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await this.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var userObj = _context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            if(userObj != null)
            {                 
                userObj.Name = user.Name;
                userObj.Username = user.Username;
                userObj.Email = user.Email;
                await this.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
