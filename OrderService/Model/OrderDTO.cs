namespace OrderService.Model
{
    public class OrderDTO
    {
        public int UserId { get; set; }

        public decimal? TotalPrice { get; set; }

        public List<OrderDetailDTO> orderDetailDTOs { get; set; }
    }
}
