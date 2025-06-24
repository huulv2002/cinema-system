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

        public async Task<bool> CreatePromotionTypeAsync(PromotionType promotion)
        {
            return await _repo.AddPromotionTypeAsync(promotion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeletePromotionAsync(id);
        }

        public async Task<IEnumerable<Promotion>> ListAllPromotionAsync()
        {
            return await _repo.ListAllPromotionAsync();
        }

        public async Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync()
        {
            return await _repo.ListAllPromotionTypeAsync();
        }

        public async Task<bool> ToggleUserActiveStatusAsync(int Id)
        {
            return await _repo.ToggleUserActiveStatusAsync(Id);
        }

        public async Task<bool> UpdatePromotionAsync(PromotionViewModel promotion)
        {
            var pro = await _repo.GetPromotionByIdAsync(promotion.Id);
            if (pro == null)
            {
                return false;
            }

            pro.Code = promotion.Code;
            pro.PromotionTypeId = promotion.PromotionTypeId;
            pro.Value = promotion.Value;
            pro.Stock=promotion.Stock;
            pro.ImageUrl = promotion.ImageUrl;
            pro.Description = promotion.Description;
            pro.StartDate = promotion.StartDate;
            pro.EndDate = promotion.EndDate;
            pro.IsActive= promotion.IsActive;

            return await _repo.UpdatePromotionAsync(pro);
        }
    }

}
