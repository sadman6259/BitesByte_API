using System;
using System.Collections.Generic;

namespace BitesByte_API.Model;

public partial class Menu
{
    public int Id { get; set; }

    public string? MenuName { get; set; }

    public string? Category { get; set; }

    public decimal? Carbs { get; set; }

    public decimal? Fat { get; set; }

    public decimal? Price { get; set; }

    public decimal? Protein { get; set; }

    public decimal? TotalCalories { get; set; }

    public string? Plan { get; set; }

    public byte[]? Image { get; set; }

    public decimal? PricePerGram { get; set; }
    public string? Subcategories { get; set; }
    public int? WeekNumber { get; set; }

}
