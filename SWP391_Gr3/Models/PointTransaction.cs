using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_Gr3.Models
{
    public class PointTransaction
    {
        [Key]
        public int TxnId { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        [StringLength(50)]
        public string TxnType { get; set; } // "EARN", "REDEEM", "EXPIRE"

        [Required]
        public int Points { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        // Navigation property
        [ForeignKey("CardId")]
        public LoyaltyCard? LoyaltyCard { get; set; }
    }
}