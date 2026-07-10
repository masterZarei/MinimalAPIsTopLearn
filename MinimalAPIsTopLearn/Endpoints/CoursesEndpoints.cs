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
        group.MapPost("/", Create).DisableAntiforgery();
        return group;
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
}
