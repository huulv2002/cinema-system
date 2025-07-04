﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddPromotionTypeAsync(PromotionType promotiontype)
        {
            await _context.PromotionTypes.AddAsync(promotiontype);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Promotion> GetPromotionByIdAsync(int id)
        {
            var pro= await _context.Promotions.FindAsync(id);
            if(pro == null)
            {
                throw new NotImplementedException();               
            }
            return pro;
        }

        public async Task<IEnumerable<Promotion>> ListAllPromotionAsync()
        {
           return await _context.Promotions.Include(c=>c.PromotionType).ToListAsync();
        }

        public async Task<IEnumerable<PromotionType>> ListAllPromotionTypeAsync()
        {
            return await _context.PromotionTypes.ToListAsync();
        }

        public async Task<bool> ToggleUserActiveStatusAsync(int Id)
        {
            var date = DateTime.Now;

            var pro = await _context.Promotions.FindAsync(Id);
            if (pro == null)
            {
                return false;
            }
            DateTime? exdate = pro.EndDate;
            if (date > exdate)
            {
                pro.IsActive = false;
                return await _context.SaveChangesAsync() > 0;
            }
            pro.IsActive = !pro.IsActive;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePromotionTypeAsync(PromotionType promotion)
        {
            _context.PromotionTypes.Update(promotion);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
