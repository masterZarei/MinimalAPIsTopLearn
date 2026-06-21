using Microsoft.AspNetCore.Cors;
using MinimalAPIsTopLearn.Entities;

var builder = WebApplication.CreateBuilder(args);

//Services Start
builder.Services.AddOpenApi();
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

//Services End

//Middleware Start
var app = builder.Build();

app.UseCors();

app.MapGet("/categories", [EnableCors(policyName: "free")] () =>
{
    return new List<CategoryInfo>
    {
        new(){Id=0, Name="Web Development"},
        new(){Id=1, Name="Mobile Development"},
        new(){Id=2, Name="Desktop Development"},
        new(){Id=3, Name="Database Adminstrator"},
    };
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