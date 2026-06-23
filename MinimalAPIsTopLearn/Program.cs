using Microsoft.AspNetCore.Cors;
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

app.MapGet("/categories", async (ICategoryRepository _repo) =>
{
    return await _repo.GetAll();
}).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)));

app.MapGet("/categories/{id:int}", async (int id, ICategoryRepository _repo) =>
{
    var category = await _repo.GetById(id);

    if (category is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(category);
});


app.MapPost("/categories", async (CategoryInfo category, ICategoryRepository _categoryRepository) =>
{
    var id = await _categoryRepository.Create(category);
    return Results.Created($"/categories/{id}", category);
});


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