using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repo;
        public PromotionService(IPromotionRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreatePromotionAsync(PromotionViewModel promotionVM)
        {
            var pro = new Promotion()
            {
                Code = promotionVM.Code,
                PromotionTypeId = promotionVM.PromotionTypeId,
                Value = promotionVM.Value,
                IsActive = promotionVM.IsActive,
                Stock = promotionVM.Stock,
                StartDate = DateTime.Now,
                EndDate = promotionVM.EndDate,
                ImageUrl = promotionVM.ImageUrl,
                Description = promotionVM.Description,
            };
            return await _repo.AddPromotionAsync(pro);
        }

        public async Task<IEnumerable<Promotion>> ListAllPromotionAsync()
        {
            return await _repo.ListAllPromotionAsync();
        }

        public async Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync()
        {
           return await _repo.ListAllPromotionTypeAsync();
        }
    }

}
