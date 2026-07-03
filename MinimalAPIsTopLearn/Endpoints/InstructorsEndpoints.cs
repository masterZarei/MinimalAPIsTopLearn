using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;

namespace MinimalAPIsTopLearn.Endpoints;

public static class InstructorsEndpoints
{
    public static RouteGroupBuilder MapInstructors(this RouteGroupBuilder group)
    {
        group.MapPost("/", Create).DisableAntiforgery();
        return group;
    }
    static async Task<Created<InstructorDTO>> Create([FromForm] InstructorCreateDTO instructorCreateDTO,
        IInstructorRepository _repo, IOutputCacheStore _cacheStore, IMapper _mapper)
    {
        var instructor = _mapper.Map<InstructorInfo>(instructorCreateDTO);
        var id = await _repo.Create(instructor);
        await _cacheStore.EvictByTagAsync("instructor-get", default);
        var instructorDto = _mapper.Map<InstructorDTO>(instructor);
        return TypedResults.Created($"/instructors/{id}", instructorDto);
    }
}
