using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlyMembers.Models
{
    public class PointTransaction
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public int PurchaseAmount { get; set; }
        public int PointsAdded { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
