using BitesByte_API.Service;
using BitesByte_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<BitesByteDbContext>(options => options.UseSqlServer(connectionString));
        
        builder.Services.AddScoped<ICaloriesCalculator, CaloriesCalculator>();
        builder.Services.AddScoped<IMenuService, MenuService>();

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

        #region getMaintainaceCalories
        app.MapPost("/getMaintainaceCalories", (ICaloriesCalculator caloriesCalculator, [FromBody] MaintainanceCaloriesDTO maintainanceCaloriesDTO) =>
        {
            return caloriesCalculator.getMaintainaceCalories(maintainanceCaloriesDTO);
        })
        .WithName("GetMaintainaceCalories")
        .WithOpenApi();
        #endregion

        #region insertmenu
        app.MapPost("/insertMenues", (IMenuService menuService, [FromBody] List<Menu> menuLst) =>
        {
            return menuService.InsertandRetrieveMenues(menuLst);
        })
        .WithName("InsertMenues")
        .WithOpenApi();
        #endregion

        app.Run();
    }
}