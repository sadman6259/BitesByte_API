using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json.Serialization;

namespace BitesByte_API.Model
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public int? GoalWeight { get; set; }

        public int? GoalBodyFat { get; set; }

        public bool? FoodAllergies { get; set; }

        public int? AvgExerciseDuration { get; set; }

       
    }
}
