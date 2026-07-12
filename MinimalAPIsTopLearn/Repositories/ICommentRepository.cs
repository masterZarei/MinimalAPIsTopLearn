using MinimalAPIsTopLearn.Entities;

namespace MinimalAPIsTopLearn.Repositories;

public interface ICommentRepository
{
    Task<List<CommentInfo>> GetAll(int courseId);
    Task<CommentInfo?> GetById(int id);
    Task<int> Create(CommentInfo comment);
    Task Delete(int id);
    Task<bool> Exists(int id);
    Task Update(CommentInfo comment);
}
