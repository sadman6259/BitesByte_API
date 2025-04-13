using Microsoft.AspNetCore.Mvc;
using BitesByte_API.Model;
using BitesByte_API.Service;
using System.Runtime.CompilerServices;

namespace BitesByte_API.Helper
{
    public static class APIHelper
    {
       
        public static void getMaintainaceCalories(WebApplication app)
        {
            #region getMaintainaceCalories
            app.MapPost("/getMaintainaceCalories", (ICaloriesCalculator caloriesCalculator, [FromBody] MaintainanceCaloriesDTO maintainanceCaloriesDTO) =>
            {
                return caloriesCalculator.getMaintainaceCalories(maintainanceCaloriesDTO);
            })
            .WithName("GetMaintainaceCalories")
            .WithOpenApi();
            #endregion
        }

        public static void insertmenu(WebApplication app)
        {
            #region insertmenu
            app.MapPost("/insertMenues", (IMenuService menuService, [FromBody] List<Menu> menuLst) =>
            {
                return menuService.InsertandRetrieveMenues(menuLst);
            })
            .WithName("InsertMenues")
            .WithOpenApi();
            #endregion
        }

        public static void registerUser(WebApplication app)
        {
            #region registeruser
            app.MapPost("/registerUser", (IUserService userService, [FromBody] UserDTO user) =>
            {
                return userService.RegisterUser(user);
            })
            .WithName("RegisterUser")
            .WithOpenApi();
            #endregion

        }
        public static void loginUser(WebApplication app)
        {
            #region loginuser
            app.MapPost("/loginUser", (IUserService userService, [FromBody] LoginUserDTO user) =>
            {
                return userService.LoginUser(user);
            })
            .WithName("LoginUser")
            .WithOpenApi();
            #endregion

        }

        public static void getavailablemenus(WebApplication app)
        {
            #region getavailablemenus
            app.MapGet("/getavailablemenus", (IMenuService menuService) =>
            {
                return menuService.GetAvailableMenu();
            })
            .WithName("GetAvailableMenus")
            .WithOpenApi();
            #endregion

        }

        public static void GetRecommendedMenuByCalorie(WebApplication app)
        {
            #region getavailablemenus
            app.MapPost("/getRecommendedMenuByCalorie", (IMenuService menuService, [FromBody] decimal? cal) =>
            {
                return menuService.GetRecommendedMenuByCalorie(cal);
            })
            .WithName("GetRecommendedMenuByCalorie")
            .WithOpenApi();
            #endregion

        }

        public static void GetRecommendedMenuByProteinCarbs(WebApplication app)
        {
            #region getavailablemenus
            app.MapGet("/getRecommendedMenuByProteinCarbs", (IMenuService menuService, decimal? protein, decimal? carbs) =>
            {
                return menuService.GetRecommendedMenuByProteinCarbs(protein,carbs);
            })
            .WithName("GetRecommendedMenuByProteinCarbs")
            .WithOpenApi();
            #endregion

        }

        public static void GetMenusBySubCategory(WebApplication app)
        {
            #region getmenusbysubcategory
            app.MapGet("/getMenusBySubCategory", (IMenuService menuService, string? subcategory, string? category) =>
            {
                return menuService.GetMenusBySubCategory(subcategory,category);
            })
            .WithName("GetMenusBySubCategory")
            .WithOpenApi();
            #endregion

        }
        public static void UpdateMenus(WebApplication app)
        {
            #region getmenusbysubcategory
            app.MapPost("/updateMenus", (IMenuService menuService, [FromBody] List<Menu> menuLst) =>
            {
                menuService.UpdateMenues(menuLst);
            })
            .WithName("UpdateMenus")
            .WithOpenApi();
            #endregion

        }

        public static void GetRecommendedMenuByProtein(WebApplication app)
        {
            #region getavailablemenus
            app.MapGet("/getRecommendedMenuByProtein", (IMenuService menuService, decimal? protein) =>
            {
                return menuService.GetRecommendedMenuByProtein(protein);
            })
            .WithName("GetRecommendedMenuByProtein")
            .WithOpenApi();
            #endregion

        }

        public static void GetRecommendedMenuByCarbs(WebApplication app)
        {
            #region getavailablemenus
            app.MapGet("/getRecommendedMenuByCarbs", (IMenuService menuService, decimal? carbs) =>
            {
                return menuService.GetRecommendedMenuByCarbs(carbs);
            })
            .WithName("GetRecommendedMenuByCarbs")
            .WithOpenApi();
            #endregion

        }
    }
}
