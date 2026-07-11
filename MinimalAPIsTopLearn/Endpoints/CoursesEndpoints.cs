using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;
using MinimalAPIsTopLearn.Services;

namespace MinimalAPIsTopLearn.Endpoints;

public static class CoursesEndpoints
{
    private readonly static string _container = "courses";
    private readonly static string _cacheTag = "courses-get";
    public static RouteGroupBuilder MapCourses(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAll)
            .CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag(_cacheTag));
        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create).DisableAntiforgery();
        group.MapPut("/{id:int}", Update).DisableAntiforgery();
        return group;
    }
    static async Task<Ok<List<CourseDTO>>> GetAll(ICourseRepository _repo, IMapper _mapper,
        int page = 1, int recordsPerPage = 10)
    {
        var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
        var courses = await _repo.GetAll(pagination);
        var courseDto = _mapper.Map<List<CourseDTO>>(courses);
        return TypedResults.Ok(courseDto);
    }
    static async Task<Results<Ok<CourseDTO>, NotFound>> GetById(int id, ICourseRepository _repo,
        IMapper _mapper)
    {
        var course = await _repo.GetById(id);
        if (course is null)
        {
            return TypedResults.NotFound();
        }
        var courseDto = _mapper.Map<CourseDTO>(course);
        return TypedResults.Ok(courseDto);
    }
    static async Task<Created<CourseDTO>> Create([FromForm] CourseCreateDTO courseCreateDTO,
        IFileStorage _fileStorage, IOutputCacheStore _outputCacheStore, IMapper _mapper,
        ICourseRepository _repo)
    {
        var course = _mapper.Map<CourseInfo>(courseCreateDTO);
        if (courseCreateDTO.Thumbnail is not null)
        {
            var url = await _fileStorage.Store(_container, courseCreateDTO.Thumbnail);
            course.Thumbnail = url;
        }
        var id = await _repo.Create(course);
        await _outputCacheStore.EvictByTagAsync(_cacheTag, default);
        var courseDto = _mapper.Map<CourseDTO>(course);
        return TypedResults.Created($"courses/{id}", courseDto);
    }
    static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] CourseCreateDTO courseCreateDTO,
        IFileStorage _fileStorage, IOutputCacheStore _outputCacheStore, IMapper _mapper,
        ICourseRepository _repo)
    {
        var course = await _repo.GetById(id);
        if (course is null)
        {
            return TypedResults.NotFound();
        }
        var courseToUpdate = _mapper.Map<CourseInfo>(courseCreateDTO);
        courseToUpdate.Id = id;
        courseToUpdate.Thumbnail = course.Thumbnail;

        if (courseCreateDTO.Thumbnail is not null)
        {
            var url = await _fileStorage.Edit(courseToUpdate.Thumbnail, _container, courseCreateDTO.Thumbnail);
            courseToUpdate.Thumbnail = url;
        }

        await _repo.Update(courseToUpdate);
        await _outputCacheStore.EvictByTagAsync(_cacheTag, default);
        return TypedResults.NoContent();
    }
}
