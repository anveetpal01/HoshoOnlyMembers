using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlyMembers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlyMembers.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Member member)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Name, member.Name),
                new Claim(ClaimTypes.Email, member.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
