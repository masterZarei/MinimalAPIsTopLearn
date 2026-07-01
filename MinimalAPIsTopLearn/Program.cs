using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalAPIsTopLearn.Data;
using MinimalAPIsTopLearn.Endpoints;
using MinimalAPIsTopLearn.Entities;
using MinimalAPIsTopLearn.Repositories;
using MinimalAPIsTopLearn.Utilities;

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
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
//Services End

//Middleware Start
var app = builder.Build();
app.UseOutputCache();
app.UseCors();


var categoriesEndpoints = app.MapGroup("/categories").MapCategories();




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
