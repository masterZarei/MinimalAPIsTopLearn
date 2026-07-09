using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Utilities.Extentions;

namespace MinimalAPIsTopLearn.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public CourseRepository(AppDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }
    public async Task<List<CourseInfo>> GetAll(PaginationDTO pagination)
    {
        var queryable = _context.Courses.AsQueryable();
        await _httpContext.HttpContext!.InsertPaginationInfoIntoResponseHeader(queryable);
        return await queryable
            .OrderBy(x => x.Title)
            .Paginate(pagination)
            .ToListAsync();
    }
    public async Task<CourseInfo?> GetById(int id)
    {
        return await _context.Courses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<int> Create(CourseInfo course)
    {
        await _context.AddAsync(course);
        await _context.SaveChangesAsync();
        return course.Id;
    }
    public async Task Update(CourseInfo course)
    {
        _context.Update(course);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        await _context.Courses.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
    public async Task<bool> Exists(int id)
    {
        return await _context.Courses.AnyAsync(x => x.Id == id);
    }

    public async Task<List<CourseInfo>?> GetByName(string title)
    {
        return await _context.Courses
            .Where(x => x.Title.Contains(title))
            .OrderBy(x => x.Title)
            .ToListAsync();
    }
}
