using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyMembers.Data;
using OnlyMembers.DTOs;
using OnlyMembers.Models;

namespace OnlyMembers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PointsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PointsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPoints(AddPointsDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return NotFound("Member not found.");

            int points = (dto.PurchaseAmount / 100) * 10;
            member.Points += points;

            var transaction = new PointTransaction
            {
                MemberId = member.Id,
                PurchaseAmount = dto.PurchaseAmount,
                PointsAdded = points
            };

            _context.PointTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Points added successfully.", totalPoints = member.Points });
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetPoints(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return NotFound("Member not found.");
            return Ok(new { totalPoints = member.Points });
        }
    }
}
