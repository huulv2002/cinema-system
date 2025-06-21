using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly Swp391Context _context;

        public PromotionRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<bool> AddPromotionAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddPromotionTypeAsync(Promotion promotion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                _context.Promotions.Remove(promotion);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public Task<Promotion> GetPromotionByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Promotion>> ListAllPromotionAsync()
        {
           return await _context.Promotions.Include(c=>c.PromotionType).ToListAsync();
        }

        public async Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync()
        {
            return await _context.PromotionTypes.ToListAsync();
        }

        public async Task<bool> UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePromotionTypeAsync(Promotion promotion)
        {
            throw new NotImplementedException();
        }
    }
}
