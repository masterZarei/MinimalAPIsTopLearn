using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsTopLearn.DTOs;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;

namespace MinimalAPIsTopLearn.Endpoints;

public static class CategoriesEndpoints
{
    public static RouteGroupBuilder MapCategories(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetCategories)
            .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("categories-get"));

        group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Created);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);

        return group;
    }


    static async Task<Ok<List<CategoryDTO>>> GetCategories(ICategoryRepository _repo, IMapper _mapper)
    {
        var categories = await _repo.GetAll();
        var categoryDtoList = _mapper.Map<List<CategoryDTO>>(categories);
        return TypedResults.Ok(categoryDtoList);
    }

    static async Task<Results<Ok<CategoryDTO>, NotFound>> GetById(int id, ICategoryRepository _repo, IMapper _mapper)
    {
        var category = await _repo.GetById(id);

        if (category is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(_mapper.Map<CategoryDTO>(category));
    }

    static async Task<Created<CategoryCreateDTO>> Created(CategoryCreateDTO categoryCreateDTO, ICategoryRepository _categoryRepository,
        IOutputCacheStore _cacheStore, IMapper _mapper)
    {
        var category = _mapper.Map<CategoryInfo>(categoryCreateDTO);
        var id = await _categoryRepository.Create(category);

        await _cacheStore.EvictByTagAsync("categories-get", default);

        var result = _mapper.Map<CategoryCreateDTO>(category);
        return TypedResults.Created($"/categories/{id}", result);
    }

    static async Task<Results<NoContent, NotFound>> Update(int id, CategoryCreateDTO categoryCreateDTO, ICategoryRepository _repo,
        IOutputCacheStore _cacheStore, IMapper _mapper)
    {
        var exists = await _repo.Exists(id);
        if (exists == false)
        {
            return TypedResults.NotFound();
        }
        var category = _mapper.Map<CategoryInfo>(categoryCreateDTO);
        category.Id = id;
        await _repo.Update(category);
        await _cacheStore.EvictByTagAsync("categories-get", default);
        return TypedResults.NoContent();
    }
    static async Task<Results<NoContent, NotFound>> Delete(int id, ICategoryRepository _repo,
        IOutputCacheStore _cacheStore)
    {
        var exists = await _repo.Exists(id);
        if (exists == false)
        {
            return TypedResults.NotFound();
        }
        await _repo.Delete(id);
        await _cacheStore.EvictByTagAsync("categories-get", default);
        return TypedResults.NoContent();
    }

}
