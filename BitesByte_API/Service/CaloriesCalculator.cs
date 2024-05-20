using BitesByte_API.Model;
using System.Linq.Expressions;
using static BitesByte_API.Helper.CaloriesHelper;

namespace BitesByte_API.Service
{
    public interface ICaloriesCalculator
    {
        decimal getMaintainaceCalories(MaintainanceCaloriesDTO maintainanceCalories);
    }
    public class CaloriesCalculator : ICaloriesCalculator
    {
        public decimal getMaintainaceCalories(MaintainanceCaloriesDTO maintainanceCalories) {
            decimal calories = 0;

            if (maintainanceCalories != null && IsValidInputforMaintainaceCalories(maintainanceCalories)) { 
               decimal BMR = 0;
               decimal weight = 0;
               int gendervariable = 0;

               if (maintainanceCalories.gender == "Male")
                    gendervariable = 5;
               else
                    gendervariable = -161;

               if (maintainanceCalories.weightMeasurer == "Kg")
                    weight = maintainanceCalories.weight / (decimal)2.205;
               else if (maintainanceCalories.weightMeasurer == "Lbs")
                    weight = maintainanceCalories.weight;
               
               BMR = (10 * weight) + ((decimal)6.25 * maintainanceCalories.height) - (5 * maintainanceCalories.age) + gendervariable;

               calories = BMR * getActivityLevel(maintainanceCalories.activityLevel);
            }
            return calories;
        }

        public decimal getActivityLevel(string activityLvl)
        {
            double activityValue = 0;
            switch (activityLvl)
            {
                case "Heavy":
                    activityValue = ActivityLevel.Heavy;
                    break;
                case "Sedentary":
                    activityValue = ActivityLevel.Sedentary;
                    break;
                case "Mild":
                    activityValue = ActivityLevel.Mild;
                    break;
                case "Moderate":
                    activityValue = ActivityLevel.Moderate;
                    break;
                case "Extreme":
                    activityValue = ActivityLevel.Extreme;
                    break;
                default:
                    activityValue = ActivityLevel.Mild;
                    break;
            }

            return (decimal)activityValue;
        }

        public bool IsValidInputforMaintainaceCalories(MaintainanceCaloriesDTO maintainanceCalories)
        {
            bool isValid = false;
            if (maintainanceCalories != null && maintainanceCalories.age > 0 &&
                maintainanceCalories.height > 0 && maintainanceCalories.weight > 0 &&
                !string.IsNullOrEmpty(maintainanceCalories.activityLevel) &&
                !string.IsNullOrEmpty(maintainanceCalories.gender) &&
                !string.IsNullOrEmpty(maintainanceCalories.weightMeasurer)
                )
                {
                return true;
            }
            return isValid;
        }
    }
}
