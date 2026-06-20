using MinimalAPIsTopLearn.Entities;

var builder = WebApplication.CreateBuilder(args);

//Services Start
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//Services End

//Middleware Start
var app = builder.Build();

app.MapGet("/categories", () =>
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