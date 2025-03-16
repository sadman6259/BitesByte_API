using System;
using System.Collections.Generic;

namespace OrderService.Model;

public partial class OrderDetail
{
    public long Id { get; set; }

    public int MenuId { get; set; }

    public int Quantity { get; set; }

    public string OrderReferenceNo { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedTime { get; set; }
    public decimal? TotalGrams { get; set; }

}
