using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories;

public interface IInstructorRepository
{
    Task<int> Create(InstructorInfo instructor);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<List<InstructorInfo>> GetAll(PaginationDTO pagination);
    Task<InstructorInfo?> GetById(int id);
    Task<List<InstructorInfo>?> GetByName(string name);
    Task Update(InstructorInfo instructor);
}