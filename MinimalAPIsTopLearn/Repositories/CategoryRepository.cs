using Microsoft.EntityFrameworkCore;
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



        public async Task<List<CategoryInfo>> GetAll()
        {
            return await _context.Categories
                //.OrderByDescending(c=>c.Name)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<CategoryInfo?> GetById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> Exists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
        public async Task Update(CategoryInfo category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

        }
        public async Task Delete(int id)
        {
            //var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            //_context.Remove(category);

            await _context.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}
