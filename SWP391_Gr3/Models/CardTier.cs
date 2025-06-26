using System.ComponentModel.DataAnnotations;

public class CardTier
{
    [Key]
    public int TierId { get; set; }

    public string? TierName { get; set; }
    public decimal MinSpending { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<LoyaltyCard> LoyaltyCards { get; set; }
}
