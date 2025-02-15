using BitesByte_API.Service;
using BitesByte_API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Helper;

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
        builder.Services.AddScoped<IUserService, UserService>();
        


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

        BitesByte_API.Helper.APIHelper.registerUser(app);
        BitesByte_API.Helper.APIHelper.loginUser(app);
        BitesByte_API.Helper.APIHelper.insertmenu(app);
        BitesByte_API.Helper.APIHelper.getavailablemenus(app);
        BitesByte_API.Helper.APIHelper.getMaintainaceCalories(app);
        BitesByte_API.Helper.APIHelper.GetRecommendedMenuByCalorie(app);

        app.Run();
    }
}