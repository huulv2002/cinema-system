using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Services
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> ListAllPromotionAsync();
        Task<bool> CreatePromotionAsync(PromotionViewModel promotion);
        Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync();
    }
}
