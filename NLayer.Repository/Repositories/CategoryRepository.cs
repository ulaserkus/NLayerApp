using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Repositories;

namespace NLayer.Repository.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetSingleCategoryWithProductsAsync(int categoryId)
        {
            return await _context.Categories.Include(c => c.Products).Where(c => c.Id == categoryId).SingleOrDefaultAsync();
        }
    }
}
