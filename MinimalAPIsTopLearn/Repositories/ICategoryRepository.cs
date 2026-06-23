using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories
{
    public interface ICategoryRepository
    {
        Task<int> Create(CategoryInfo category);
        Task<CategoryInfo?> GetById(int id);
        Task<List<CategoryInfo>> GetAll();
    }
}
