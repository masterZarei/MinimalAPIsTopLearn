using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories;

public interface IInstructorRepository
{
    Task<int> Create(InstructorInfo instructor);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<List<InstructorInfo>> GetAll();
    Task<InstructorInfo?> GetById(int id);
    Task Update(InstructorInfo instructor);
}