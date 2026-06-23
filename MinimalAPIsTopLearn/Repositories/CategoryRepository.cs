using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> Create(CategoryInfo category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }
    }
}
