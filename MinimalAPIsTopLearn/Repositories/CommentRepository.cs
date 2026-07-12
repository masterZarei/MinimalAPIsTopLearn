using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories;

public class CommentRepository(AppDbContext _context) : ICommentRepository
{
    public async Task<List<CommentInfo>> GetAll(int courseId)
    {
        return await _context.Comments.Where(x => x.CourseId == courseId).ToListAsync();
    }
    public async Task<CommentInfo?> GetById(int id)
    {
        return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<int> Create(CommentInfo instructor)
    {
        await _context.AddAsync(instructor);
        await _context.SaveChangesAsync();
        return instructor.Id;
    }
    public async Task Update(CommentInfo instructor)
    {
        _context.Update(instructor);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        await _context.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
    public async Task<bool> Exists(int id)
    {
        return await _context.Comments.AnyAsync(x => x.Id == id);
    }
}
