using SWP391_Gr3.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class LoyaltyCard
{
    public int CardId { get; set; }

    public int UserId { get; set; } // ✅ thay vì CustomerId

    public string? CardNumber { get; set; }
    public string? CardType { get; set; }
    public DateTime? RegisterDate { get; set; }
    public string? Status { get; set; }

    public int TierId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("TierId")]
    public CardTier? Tier { get; set; }
}
