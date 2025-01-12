using System;
using System.Collections.Generic;

namespace BitesByte_API.Model;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? GoalWeight { get; set; }

    public int? GoalBodyFat { get; set; }

    public bool? FoodAllergies { get; set; }

    public int? AvgExerciseDuration { get; set; }
}
