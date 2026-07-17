using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;

namespace MinimalAPIsTopLearn.Endpoints;

public static class CommentsEndpoints
{
    private readonly static string _cacheTag = "comments-get";
    public static RouteGroupBuilder MapComments(this RouteGroupBuilder builder)
    {
        builder.MapPost("/", Create);
        return builder;
    }
    static async Task<Results<Created<CommentDTO>, NotFound>> Create(int courseId, 
        CommentCreateDTO commentCreateDTO, ICourseRepository _courseRepo, ICommentRepository _commentRepo,
        IOutputCacheStore _outputCacheStore,IMapper _mapper)
    {
        if(!await _courseRepo.Exists(courseId))
        {
            return TypedResults.NotFound();
        }
        var comment = _mapper.Map<CommentInfo>(commentCreateDTO);
        comment.CourseId = courseId;
        var id = await _commentRepo.Create(comment);
        await _outputCacheStore.EvictByTagAsync(_cacheTag, default);
        var commentDto = _mapper.Map<CommentDTO>(comment);
        return TypedResults.Created($"/comments/{id}", commentDto);
    }
}
