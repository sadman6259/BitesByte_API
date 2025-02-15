namespace BitesByte_API.Model
{
    public class RecommendedMealsDTO
    {
        public int Totalcalories {  get; set; }
        public List<Menu> recommendedMenueList { get; set; }
    }
}
