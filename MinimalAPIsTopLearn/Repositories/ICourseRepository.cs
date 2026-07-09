using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories;

public interface ICourseRepository
{
    Task<int> Create(CourseInfo course);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task<List<CourseInfo>> GetAll(PaginationDTO pagination);
    Task<CourseInfo?> GetById(int id);
    Task<List<CourseInfo>?> GetByName(string title);
    Task Update(CourseInfo course);
}
