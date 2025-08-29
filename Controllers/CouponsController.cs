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
    public class CouponsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CouponsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("redeem")]
        public async Task<IActionResult> RedeemPoints(RedeemDto dto)
        {
            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return NotFound("Member not found.");
            if (dto.PointsToRedeem % 100 != 0)
                return BadRequest("Points can only be redeemed in multiples of 100.");
            if (member.Points < dto.PointsToRedeem)
                return BadRequest("Not enough points.");

            int couponsWorth = (dto.PointsToRedeem / 100) * 10;

            var coupon = new Coupon
            {
                MemberId = member.Id,
                PointsRedeemed = dto.PointsToRedeem,
                ValueInRupees = couponsWorth,
                CouponCode = GenerateCouponCode()
            };

            member.Points -= dto.PointsToRedeem;
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Coupon redeemed successfully.", coupon.CouponCode, coupon.ValueInRupees, remainingPoints = member.Points });
        }

        [HttpGet("{memberId}")]
        public async Task<IActionResult> GetCoupons(int memberId)
        {
            var coupons = await _context.Coupons.Where(c => c.MemberId == memberId).ToListAsync();
            return Ok(coupons);
        }

        private string GenerateCouponCode()
        {
            return "CUP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }
}
