using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Utilities.Extentions;

namespace MinimalAPIsTopLearn.Repositories;

public class InstructorRepository : IInstructorRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public InstructorRepository(AppDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }
    public async Task<List<InstructorInfo>> GetAll(PaginationDTO pagination)
    {
        var queryable = _context.Instructors.AsQueryable();
        await _httpContext.HttpContext!.InsertPaginationInfoIntoResponseHeader(queryable);
        return await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync(); 
    }
    public async Task<InstructorInfo?> GetById(int id)
    {
        return await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<int> Create(InstructorInfo instructor)
    {
        await _context.AddAsync(instructor);
        await _context.SaveChangesAsync();
        return instructor.Id;
    }
    public async Task Update(InstructorInfo instructor)
    {
        _context.Update(instructor);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        await _context.Instructors.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
    public async Task<bool> Exists(int id)
    {
        return await _context.Instructors.AnyAsync(x => x.Id == id);
    }

    public async Task<List<InstructorInfo>?> GetByName(string name)
    {
        return await _context.Instructors
            .Where(x => x.Name.Contains(name))
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}
