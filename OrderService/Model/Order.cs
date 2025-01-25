using System;
using System.Collections.Generic;

namespace OrderService.Model;

public partial class Order
{
    public long Id { get; set; }

    public string? OrderReferenceNo { get; set; }

    public int UserId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedDate { get; set; }
}
