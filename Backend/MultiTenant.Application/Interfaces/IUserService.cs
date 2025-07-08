using MultiTenant.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.Application.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllUsers();
        public Task<UserDto> GetUserById(int id);
        public Task AddUser(UserDto user);
        public Task UpdateUser(UserDto user);
        public Task DeleteUser(int id);
    }
}
