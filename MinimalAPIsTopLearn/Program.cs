using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Services Start

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.WithOrigins(builder.Configuration["AllowedOrigins"]!).AllowAnyMethod()
        .AllowAnyHeader();
    });
    options.AddPolicy("free", config =>
    {
        config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

//Services End

//Middleware Start
var app = builder.Build();
app.UseOutputCache();
app.UseCors();

#region Categories Endpoints
var categoriesEndpoints = app.MapGroup("/categories");

categoriesEndpoints.MapGet("/", GetCategories)
    .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("categories-get"));

categoriesEndpoints.MapGet("/{id:int}", GetById);
categoriesEndpoints.MapPost("/", Created);
categoriesEndpoints.MapPut("/{id:int}", Update);
categoriesEndpoints.MapDelete("/{id:int}", Delete);
#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
//Middleware End

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