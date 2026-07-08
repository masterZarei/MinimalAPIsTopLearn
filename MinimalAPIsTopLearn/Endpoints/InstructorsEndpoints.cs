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
        group.MapPut("/{id:int}", Update).DisableAntiforgery();
        group.MapPost("/", Create).DisableAntiforgery();
        return group;
    }
    static async Task<Ok<List<InstructorDTO>>> GetAll(IInstructorRepository _repo, IMapper _mapper,
        int page = 1, int recordsPerPage = 10)
    {
        var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
        var instructors = await _repo.GetAll(pagination);
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

    static async Task<Results<NoContent, NotFound>> Update(int id, [FromForm] InstructorCreateDTO instructorCreateDTO,
        IInstructorRepository _repo, IOutputCacheStore _cacheStore, IMapper _mapper, IFileStorage _fileStorage)
    {
        var instructor = await _repo.GetById(id);
        if (instructor is null)
        {
            return TypedResults.NotFound();
        }
        var instructorToUpdate = _mapper.Map<InstructorInfo>(instructorCreateDTO);
        instructorToUpdate.Id = id;
        instructorToUpdate.Picture = instructor.Picture;

        if (instructorCreateDTO.Picture is not null)
        {
            var url = await _fileStorage.Edit(instructorToUpdate.Picture, _container,
                instructorCreateDTO.Picture);
            instructorToUpdate.Picture = url;
        }
        await _repo.Update(instructorToUpdate);
        await _cacheStore.EvictByTagAsync(_cacheTag, default);
        return TypedResults.NoContent();
    }
}
