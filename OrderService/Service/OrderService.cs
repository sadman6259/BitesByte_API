using OrderService.Model;
using OrderService.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderService.Service
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(OrderDTO order);
        Task<List<OrderDetail>> CreateOrderDetail(List<OrderDetailDTO> orderDetailDtoLst, string orderRef);

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

            orderRepository.CreateOrder(ordernew);

            await CreateOrderDetail(order.orderDetailDTOs,ordernew.OrderReferenceNo);


            return ordernew;
        }

        public async Task<List<OrderDetail>> CreateOrderDetail(List<OrderDetailDTO> orderDetailDtoLst, string orderRef)
        {

            List<OrderDetail> newOrderDetailLst = new List<OrderDetail>();

            foreach (var orderDetailDto in orderDetailDtoLst)
            {
                OrderDetail newOrderDetail = new OrderDetail();
                newOrderDetail.TotalPrice = orderDetailDto.TotalPrice;
                newOrderDetail.Quantity = orderDetailDto.Quantity;
                newOrderDetail.MenuId = orderDetailDto.menuId;
                newOrderDetail.CreatedTime = DateTime.Now;
                newOrderDetail.OrderReferenceNo = orderRef;
                newOrderDetailLst.Add(newOrderDetail);

            }
            
            return orderRepository.CreateOrderDetail(newOrderDetailLst);
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
    }
}
