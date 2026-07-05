using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;
using MinimalAPIsTopLearn.Services;

namespace MinimalAPIsTopLearn.Endpoints;

public static class InstructorsEndpoints
{
    private readonly static string _container = "instructors";
    private readonly static string _cacheTag = "instructor-get";
    public static RouteGroupBuilder MapInstructors(this RouteGroupBuilder group)
    {
        group.MapGet("/",GetAll).CacheOutput(c=>c.Expire(TimeSpan.FromMinutes(1)).Tag(_cacheTag));
        group.MapGet("/{id:int}", GetById);
        group.MapGet("/{name}", GetByName);
        group.MapPost("/", Create).DisableAntiforgery();
        return group;
    }
    static async Task<Ok<List<InstructorDTO>>> GetAll(IInstructorRepository _repo, IMapper _mapper)
    {
        var instructors = await _repo.GetAll();
        var instructorDto = _mapper.Map<List<InstructorDTO>>(instructors);
        return TypedResults.Ok(instructorDto);

    }
    static async Task<Results<Ok<InstructorDTO>,NotFound>> GetById(int id,IInstructorRepository _repo, IMapper _mapper)
    {
        var instructor = await _repo.GetById(id);
        if (instructor is null)
        {
            return TypedResults.NotFound();
        }
        var instructorDto = _mapper.Map<InstructorDTO>(instructor);
        return TypedResults.Ok(instructorDto);
    }
    static async Task<Ok<List<InstructorDTO>>> GetByName(string name, IInstructorRepository _repo, IMapper _mapper)
    {
        var instructor = await _repo.GetByName(name);
       
        var instructorDto = _mapper.Map<List<InstructorDTO>>(instructor);
        return TypedResults.Ok(instructorDto);
    }
    static async Task<Created<InstructorDTO>> Create([FromForm] InstructorCreateDTO instructorCreateDTO,
        IInstructorRepository _repo, IOutputCacheStore _cacheStore, IMapper _mapper, IFileStorage _fileStorage)
    {
        var instructor = _mapper.Map<InstructorInfo>(instructorCreateDTO);

        if (instructorCreateDTO.Picture is not null)
        {
            var url = await _fileStorage.Store(_container, instructorCreateDTO.Picture);
            instructor.Picture = url;
        }

        var id = await _repo.Create(instructor);
        await _cacheStore.EvictByTagAsync(_cacheTag, default);
        var instructorDto = _mapper.Map<InstructorDTO>(instructor);
        return TypedResults.Created($"/instructors/{id}", instructorDto);
    }
}
