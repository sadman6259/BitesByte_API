namespace OrderService.Model
{
    public class OrderDetailDTO
    {
        public int menuId { get; set; }
        public int Quantity { get; set; }

        public decimal? TotalPrice { get; set; }
        public decimal? TotalGrams { get; set; }

    }
}
