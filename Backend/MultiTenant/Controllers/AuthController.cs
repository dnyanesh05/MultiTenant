using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenant.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "t1admin" && request.Password == "t1admin")
            {
                var token = GenerateJwtToken("t1admin", "tenant1");
                return Ok(new { token });
            }
            else if (request.Username == "t2admin" && request.Password == "t2admin")
            {
                var token = GenerateJwtToken("t2admin", "tenant2");
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string tenantId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("tenantId", tenantId)
            };

            var identity = new ClaimsIdentity(claims, "Custom"); // "Custom" is the authentication type
            var principal = new ClaimsPrincipal(identity);

            // For ASP.NET Core:
            HttpContext.User = principal;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
