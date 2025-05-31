using OrderService.Model;
using OrderService.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderService.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(OrderDTO order);
        Task<Order> CreateOrderGuest(GuestOrderDTO order);

        Task<List<OrderDetail>> CreateOrderDetail(List<OrderDetail> orderDetailDtoLst, string orderRef);

        Task<string> GenerateOrderRef();

        Task<OrderWithDetailDTO> GetOrderByRefNo(string orderRef);


    }
    public class OrderService : IOrderService
    {
        public IOrderRepository orderRepository { get; set; }

        public OrderService(IOrderRepository _orderRepository) {
            orderRepository = _orderRepository;
        }
        public async Task<Order> CreateOrder(OrderDTO order)
        {
            if(order == null) throw new ArgumentNullException("order");

            Order ordernew = new Order();
            ordernew.UserId = order.UserId;
            ordernew.TotalPrice = order.TotalPrice;
            ordernew.CreatedDate = DateTime.Now;
            ordernew.OrderReferenceNo = await GenerateOrderRef();

            await orderRepository.CreateOrder(ordernew);

            //await CreateOrderDetail(order.orderDetailDTOs,ordernew.OrderReferenceNo);


            return ordernew;
        }

        public async Task<List<OrderDetail>> CreateOrderDetail(List<OrderDetail> orderDetailLst, string orderRef)
        {

            List<OrderDetail> newOrderDetailLst = new List<OrderDetail>();

            foreach (var orderDetail in orderDetailLst)
            {
                orderDetail.CreatedTime = DateTime.Now;
                orderDetail.OrderReferenceNo = orderRef;
                newOrderDetailLst.Add(orderDetail);

            }
            
            return await orderRepository.CreateOrderDetail(newOrderDetailLst);
        }

        public async Task<string> GenerateOrderRef() { 
            string orderRef = string.Empty;
            orderRef = "ORD" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + orderRepository.GetSequenceNo();
            
            return orderRef;
        }

        public async Task<OrderWithDetailDTO> GetOrderByRefNo(string orderRef)
        {
            return orderRepository.GetOrderByRefNo(orderRef);
        }

        public async Task<Order> CreateOrderGuest(GuestOrderDTO guestOrder)
        {
            try
            {
                if (guestOrder == null || (guestOrder != null && (guestOrder.Order == null || guestOrder.guestUser == null || guestOrder.OrderDetail.Count < 1))) throw new ArgumentNullException("order");

                await orderRepository.CreateGuestUser(guestOrder.guestUser);
                
                orderRepository.SaveChanges(); // need to remove it later

                Order ordernew = new Order();
                ordernew = guestOrder.Order;
                ordernew.CreatedDate = DateTime.Now;
                ordernew.IsGuestUser = true;
                ordernew.OrderReferenceNo = await GenerateOrderRef();
                ordernew.UserId = guestOrder.guestUser.Id;

                await orderRepository.CreateOrder(ordernew);

                await CreateOrderDetail(guestOrder.OrderDetail, ordernew.OrderReferenceNo);

                orderRepository.SaveChanges();

                return ordernew;
            }
            catch (Exception) {
                
                throw ;
            }
            
        }
    }
}
