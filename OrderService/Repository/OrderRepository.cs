using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
        List<OrderDetail> CreateOrderDetail(List<OrderDetail> orderDetailLst);

        int GetSequenceNo();
        OrderWithDetailDTO GetOrderByRefNo(string orderRef);
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
        public OrderWithDetailDTO GetOrderByRefNo(string orderRef)
        {
            var query = bitesByteDbContext.Orders   
           .Join(bitesByteDbContext.OrderDetails, 
              order => order.OrderReferenceNo,        
              ordDetail => ordDetail.OrderReferenceNo,   
              (order, ordDetail) => new { order = order, ordDetail = ordDetail })
           .Join(bitesByteDbContext.Users,
              ord => ord.order.UserId,
              user => user.Id,
              (ord, user) => new { ord = ord, user = user })
           .Join(bitesByteDbContext.Menus,
              ord => ord.ord.ordDetail.MenuId,
              menu => menu.Id,
              (ord, menu) => new { ord = ord, menu = menu })

           .Where(orderObj => orderObj.ord.ord.order.OrderReferenceNo == orderRef).ToList();

            OrderWithDetailDTO orderWithDetailDTO = new OrderWithDetailDTO();
            List<OrderChild> orderChildren = new List<OrderChild>();

            orderWithDetailDTO.OrderReferenceNo = query.FirstOrDefault().ord.ord.order.OrderReferenceNo;
            orderWithDetailDTO.TotalPrice = query.FirstOrDefault().ord.ord.order.TotalPrice;
            orderWithDetailDTO.UserName = query.FirstOrDefault().ord.user.Name;
            orderWithDetailDTO.CreatedDate = query.FirstOrDefault().ord.ord.order.CreatedDate;

            foreach (var item in query)
            {
                OrderChild orderChild = new OrderChild();

                orderChild.MenuName = item.menu.MenuName;
                orderChild.TotalPrice = item.ord.ord.ordDetail.TotalPrice;
                orderChild.Quantity = item.ord.ord.ordDetail.Quantity;

                orderChildren.Add(orderChild);
            }

            orderWithDetailDTO.orderChildren = orderChildren;

            return orderWithDetailDTO;
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
