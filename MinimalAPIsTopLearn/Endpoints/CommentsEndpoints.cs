using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;

namespace MinimalAPIsTopLearn.Endpoints;

public static class CommentsEndpoints
{
    public static RouteGroupBuilder MapComments(this RouteGroupBuilder builder)
    {
        builder.MapGet("/", GetAll);
        builder.MapGet("/{id:int}", GetById);
        builder.MapPost("/", Create);
        return builder;
    }
    static async Task<Results<Ok<List<CommentDTO>>, NotFound>> GetAll(int courseId,
        ICourseRepository _courseRepo, ICommentRepository _commentRepo,
        IMapper _mapper)
    {
        if (!await _courseRepo.Exists(courseId))
        {
            return TypedResults.NotFound();
        }
        var comments = await _commentRepo.GetAll(courseId);
        var commentDto = _mapper.Map<List<CommentDTO>>(comments);
        return TypedResults.Ok(commentDto);
    }
    static async Task<Results<Ok<CommentDTO>, NotFound>> GetById(int id, int courseId,
        ICourseRepository _courseRepo, ICommentRepository _commentRepo,
        IMapper _mapper)
    {
        if (!await _courseRepo.Exists(courseId))
        {
            return TypedResults.NotFound();
        }
        var comment = await _commentRepo.GetById(id);
        if (comment is null)
        {
            return TypedResults.NotFound();
        }
        var commentDto = _mapper.Map<CommentDTO>(comment);
        return TypedResults.Ok(commentDto);
    }

    static async Task<Results<Created<CommentDTO>, NotFound>> Create(int courseId,
        CommentCreateDTO commentCreateDTO, ICourseRepository _courseRepo, ICommentRepository _commentRepo,
        IMapper _mapper)
    {
        if (!await _courseRepo.Exists(courseId))
        {
            return TypedResults.NotFound();
        }
        var comment = _mapper.Map<CommentInfo>(commentCreateDTO);
        comment.CourseId = courseId;
        var id = await _commentRepo.Create(comment);
        var commentDto = _mapper.Map<CommentDTO>(comment);
        return TypedResults.Created($"/comments/{id}", commentDto);
    }
}
