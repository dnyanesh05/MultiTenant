//using AutoMapper;
using AutoMapper;
using MultiTenant.Application.DTOs;
using MultiTenant.Application.Interfaces;
using MultiTenant.Domain.Entities;
using MultiTenant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenant.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        //public UserService(IUserRepository userRepository, IMapper mapper)
        //{
        //    _userRepository = userRepository;
        //    _mapper = mapper;
        //}

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUser(UserDto userDto)
        {
            //var user = _mapper.Map<User>(userDto);
            var user = new User
            {
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email
            };
            await _userRepository.AddAsync(user);
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            //var userDtos = _mapper.Map<List<UserDto>>(users);
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email
            }).ToList();
            return userDtos;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            //var userDto = _mapper.Map<UserDto>(user);
            if (user == null)
            {
                return null; // or throw an exception, depending on your error handling strategy
            }
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email
            };
            return userDto;
        }

        public async Task UpdateUser(UserDto user)
        {
            await _userRepository.UpdateAsync(new User
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Email = user.Email
            });
        }

        public async Task DeleteUser(int id)
        {   
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(id);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
