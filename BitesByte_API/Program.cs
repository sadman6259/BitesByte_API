using BitesByte_API.Service;
using BitesByte_API.Model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICaloriesCalculator, CaloriesCalculator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
else
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


