using BitesByte_API.Service;
using BitesByte_API.Model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICaloriesCalculator, CaloriesCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/getMaintainaceCalories", (ICaloriesCalculator caloriesCalculator , [FromBody]MaintainanceCaloriesDTO maintainanceCaloriesDTO) =>
{
    return caloriesCalculator.getMaintainaceCalories(maintainanceCaloriesDTO);
})
.WithName("GetMaintainaceCalories")
.WithOpenApi();

app.Run();


