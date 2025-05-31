using System;
using System.Collections.Generic;

namespace OrderService.Model;

public partial class GuestUser
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? Street { get; set; }

    public string? ApartmentUnit { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostCode { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
}
