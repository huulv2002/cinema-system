using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Services
{
    public interface IPromotionService
    {
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleUserActiveStatusAsync(int Id);
        Task<IEnumerable<Promotion>> ListAllPromotionAsync();
        Task<bool> CreatePromotionAsync(PromotionViewModel promotion);
        Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync();
        Task<bool> CreatePromotionTypeAsync(PromotionType promotion);
        Task<bool> UpdatePromotionAsync(PromotionViewModel promotion);
    }
}
