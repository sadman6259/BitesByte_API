namespace OrderService.Model
{
    public class GuestOrderDTO
    {
       public GuestUser guestUser { get; set; }
       public Order Order { get; set; }
       public List<OrderDetail> OrderDetail { get; set; }

    }
}
