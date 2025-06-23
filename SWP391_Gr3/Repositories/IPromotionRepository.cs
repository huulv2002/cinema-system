using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public interface IPromotionRepository
    {
        Task<bool> ToggleUserActiveStatusAsync(int Id); // active
        Task<Promotion> GetPromotionByIdAsync(int id);
        Task<bool> DeletePromotionAsync(int id);
        Task<IEnumerable<Promotion>> ListAllPromotionAsync();
        Task<bool> AddPromotionAsync(Promotion promotion);
        Task<bool> UpdatePromotionAsync(Promotion promotion);

        Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync();
        Task<bool> AddPromotionTypeAsync(Promotion promotion);
        Task<bool> UpdatePromotionTypeAsync(Promotion promotion);
    }
}
