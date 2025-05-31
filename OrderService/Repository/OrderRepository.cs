using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        Task<GuestUser> CreateGuestUser(GuestUser guestUser);

        Task<List<OrderDetail>> CreateOrderDetail(List<OrderDetail> orderDetailLst);
        void SaveChanges();
        int GetSequenceNo();
        OrderWithDetailDTO GetOrderByRefNo(string orderRef);
        public void BeginTran();
        public void CommitTran();
        public void RollbackTran();

    }
    public class OrderRepository : IOrderRepository
    {
        private readonly BitesByteDbContext bitesByteDbContext;

        public OrderRepository(BitesByteDbContext _bitesByteDbContext)
        {
            bitesByteDbContext = _bitesByteDbContext;
        }

        public void SaveChanges()
        {
            bitesByteDbContext.SaveChanges();
        }

        public void BeginTran()
        {
           bitesByteDbContext.Database.BeginTransactionAsync();
        }

        public void CommitTran()
        {
            bitesByteDbContext.Database.CommitTransactionAsync();
        }
        public void RollbackTran()
        {
            bitesByteDbContext.Database.RollbackTransaction();
        }

        public async Task<GuestUser> CreateGuestUser(GuestUser guestUser) {
            await bitesByteDbContext.GuestUsers.AddAsync(guestUser);
            return guestUser;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            await bitesByteDbContext.Orders.AddAsync(order);
            //bitesByteDbContext.SaveChanges();
            return order;
        }
        public async Task< List<OrderDetail>> CreateOrderDetail(List<OrderDetail> orderDetailLst)
        {

            await bitesByteDbContext.OrderDetails.AddRangeAsync(orderDetailLst);
            //bitesByteDbContext.SaveChanges();
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
