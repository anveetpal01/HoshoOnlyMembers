using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlyMembers.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public int PointsRedeemed { get; set; }
        public int ValueInRupees { get; set; }
        public string CouponCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
