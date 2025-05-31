using System;
using System.Collections.Generic;

namespace OrderService.Model;

public partial class Order
{
    public long Id { get; set; }

    public string? OrderReferenceNo { get; set; }

    public int? UserId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? DeliveryDateTime { get; set; }

    public string? Remarks { get; set; }

    public string? DeliveryMethod { get; set; }

    public string? PickupLocation { get; set; }

    public string? PaymentMethod { get; set; }

    public string? CardNumber { get; set; }

    public bool? IsPaymentDone { get; set; }

    public bool? IsGuestUser { get; set; }

}
