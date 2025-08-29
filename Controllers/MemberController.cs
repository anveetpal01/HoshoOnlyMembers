using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyMembers.Data;
using OnlyMembers.DTOs;
using OnlyMembers.Models;
using OnlyMembers.Services;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OnlyMembers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public MemberController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(MemberRegisterDto dto)
        {
            if (await _context.Members.AnyAsync(m => m.Email == dto.Email || m.Mobile == dto.Mobile))
                return BadRequest("User with this email or mobile already exists.");


            var hasher = new PasswordHasher<Member>();

            var member = new Member
            {
                Name = dto.Name,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Otp = "1234", // static dummy OTP
                IsVerified = false
            };
            member.PasswordHash = hasher.HashPassword(member, dto.Password);
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registered successfully, please verify OTP.", memberId = member.Id, otp = member.Otp });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp(MemberVerifyDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return NotFound("Member not found.");

            if (member.Otp != dto.Otp) return BadRequest("Invalid OTP.");

            member.IsVerified = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account verified successfully." });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] MemberLoginDto dto)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Name == dto.Name);
            if (member == null) return Unauthorized("Invalid name");
            if (!member.IsVerified) return Unauthorized("Member not verified.");

            var hasher = new PasswordHasher<Member>();
            var result = hasher.VerifyHashedPassword(member, member.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid name or password.");

            var token = _jwtService.GenerateToken(member);

            return Ok(new { token, member.Id, member.Name });
        }
    }
}
