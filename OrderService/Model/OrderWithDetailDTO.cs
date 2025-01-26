namespace OrderService.Model
{
    public class OrderWithDetailDTO
    {
        public string? OrderReferenceNo { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? UserName { get; set; }


        public List<OrderChild> orderChildren { get; set; }

        public DateTime? CreatedDate { get; set; }


    }

    public class OrderChild
    {

        public string? MenuName { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
