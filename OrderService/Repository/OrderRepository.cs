using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderService.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
        List<OrderDetail> CreateOrderDetail(List<OrderDetail> orderDetailLst);

        int GetSequenceNo();
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly BitesByteDbContext bitesByteDbContext;

        public OrderRepository(BitesByteDbContext _bitesByteDbContext)
        {
            bitesByteDbContext = _bitesByteDbContext;
        }
        public Order CreateOrder(Order order) {
            bitesByteDbContext.Orders.Add(order);
            bitesByteDbContext.SaveChanges();
            return order;
        }
        public List<OrderDetail> CreateOrderDetail(List<OrderDetail> orderDetailLst)
        {

            bitesByteDbContext.OrderDetails.AddRange(orderDetailLst);
            bitesByteDbContext.SaveChanges();
            return orderDetailLst;
        }

        public int GetSequenceNo()
        {
            try
            {

                var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
                p.Direction = System.Data.ParameterDirection.Output;
                bitesByteDbContext.Database.ExecuteSqlRaw("set @result = next value for OrderSequence", p);
                var nextVal = (int)p.Value;
                

                return nextVal;
            }
            catch(Exception ex) 
            { 
              throw;
            }
           
        }

    }
}
