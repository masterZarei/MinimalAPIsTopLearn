using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
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


    static async Task<Ok<List<CategoryInfo>>> GetCategories(ICategoryRepository _repo)
    {
        var categories = await _repo.GetAll();
        return TypedResults.Ok(categories);
    }

    static async Task<Results<Ok<CategoryInfo>, NotFound>> GetById(int id, ICategoryRepository _repo)
    {
        var category = await _repo.GetById(id);

        if (category is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(category);
    }

    static async Task<Created<CategoryInfo>> Created(CategoryInfo category, ICategoryRepository _categoryRepository,
        IOutputCacheStore _cacheStore)
    {
        var id = await _categoryRepository.Create(category);

        await _cacheStore.EvictByTagAsync("categories-get", default);

        return TypedResults.Created($"/categories/{id}", category);
    }

    static async Task<Results<NoContent, NotFound>> Update(int id, CategoryInfo category, ICategoryRepository _repo,
        IOutputCacheStore _cacheStore)
    {
        var exists = await _repo.Exists(id);
        if (exists == false)
        {
            return TypedResults.NotFound();
        }
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
