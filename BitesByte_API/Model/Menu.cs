using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitesByte_API.Model;

public partial class Menu
{
    [Key]
    public int Id { get; set; }

    public string? MenuName { get; set; }

    public decimal? Price { get; set; }

    public byte[]? Image { get; set; }

    public string? Category { get; set; }

    public decimal? Carbs { get; set; }

    public decimal? Protein { get; set; }

    public decimal? Fat { get; set; }

    public decimal? TotalCalories { get; set; }

    public string? Plan { get; set; }


}
