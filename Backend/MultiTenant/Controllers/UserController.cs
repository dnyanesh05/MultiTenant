using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Application.Interfaces;
using MultiTenant.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MultiTenant.Api.Controllers
{
    [Authorize]
    [ApiController]    
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPost("add")]
        public async Task<IActionResult>Add([FromBody]UserDto user)
        {
            await _userService.AddUser(user);
            return Ok();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] UserDto user)
        {
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}
