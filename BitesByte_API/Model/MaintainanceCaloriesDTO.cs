namespace BitesByte_API.Model
{
    public class MaintainanceCaloriesDTO
    {
        public decimal weight {  get; set; }
        public int height { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string activityLevel { get; set; }

        public string weightMeasurer { get; set; }

    }
}
