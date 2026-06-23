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

app.MapGet("/categories", () =>
{
    return new List<CategoryInfo>
    {
        new(){Id=0, Name="Web Development"},
        new(){Id=1, Name="Mobile Development"},
        new(){Id=2, Name="Desktop Development"},
        new(){Id=3, Name="Database Adminstrator"},
    };
}).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(15)));


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