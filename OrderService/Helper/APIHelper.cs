using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.Service;
using System.Runtime.CompilerServices;

namespace OrderService.Helper
{
    public static class APIHelper
    {
       
        public static void placeOrder(WebApplication app)
        {
            #region placeorder
            app.MapPost("/placeorder", (IOrderService orderService, [FromBody] OrderDTO order) =>
            {
                return orderService.CreateOrder(order);
            })
            .WithName("PlaceOrder")
            .WithOpenApi();
            #endregion
        }


    }
}
