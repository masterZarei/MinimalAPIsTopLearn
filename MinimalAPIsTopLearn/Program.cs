using Microsoft.AspNetCore.Cors;
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

categoriesEndpoints.MapGet("/", async (ICategoryRepository _repo) =>
{
    return await _repo.GetAll();
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("categories-get"));

categoriesEndpoints.MapGet("/{id:int}", async (int id, ICategoryRepository _repo) =>
{
    var category = await _repo.GetById(id);

    if (category is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(category);
});


categoriesEndpoints.MapPost("/", async (CategoryInfo category, ICategoryRepository _categoryRepository, IOutputCacheStore _cacheStore) =>
{
    var id = await _categoryRepository.Create(category);

    await _cacheStore.EvictByTagAsync("categories-get", default);

    return Results.Created($"/categories/{id}", category);
});

categoriesEndpoints.MapPut("/{id:int}", async (int id, CategoryInfo category, ICategoryRepository _repo, IOutputCacheStore _cacheStore) =>
{
    var exists = await _repo.Exists(id);
    if (exists == false)
    {
        return Results.NotFound();
    }
    await _repo.Update(category);
    await _cacheStore.EvictByTagAsync("categories-get", default);
    return Results.NoContent();
});

categoriesEndpoints.MapDelete("/{id:int}", async (int id, ICategoryRepository _repo, IOutputCacheStore _cacheStore) =>
{
    var exists = await _repo.Exists(id);
    if (exists == false)
    {
        return Results.NotFound();
    }
    await _repo.Delete(id);
    await _cacheStore.EvictByTagAsync("categories-get", default);
    return Results.NoContent();
});

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