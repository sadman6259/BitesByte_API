using BitesByte_API.Model;

namespace BitesByte_API.Service
{
    public interface IMenuService
    {
        List<Menu> InsertandRetrieveMenues(List<Menu> menus);
        List<Menu> GetAvailableMenu();

        List<Menu> GetRecommendedMenuByCalorie(int cal);
    }
    public class MenuService : IMenuService
    {
        private readonly BitesByteDbContext bitesByteDbContext;

        private readonly int ConstAdvforStandard = 50;

        private readonly int ConstAdvforMacro = 80;
        public MenuService(BitesByteDbContext _bitesByteDbContext) { 
          this.bitesByteDbContext = _bitesByteDbContext;
        }
        public List<Menu> InsertandRetrieveMenues(List<Menu> menus)
        {
            try
            {
                List<Menu> menuList = new List<Menu>();

                if (menus.Count > 0)
                {
                    bitesByteDbContext.Menus.AddRange(menus);
                    bitesByteDbContext.SaveChanges();
                }
                menuList = bitesByteDbContext.Menus.ToList();
                return menuList;
            }
            catch (Exception ex) {
                throw;
            }
           
        }

        public List<Menu> GetAvailableMenu()
        {
            try
            {
                return bitesByteDbContext.Menus.ToList();

            }
            catch (Exception) { throw;  }
        }

        public List<Menu> GetRecommendedMenuByCalorie(int cal)
        {
            try
            {
                List<Menu> allmenues =  bitesByteDbContext.Menus.ToList();

                // filter all menues less then target cal
                allmenues = allmenues.Where(x => x.TotalCalories <= cal).OrderBy(x => x.TotalCalories).ToList();

                // filter standard & macro
                List<Menu> standardMenues = allmenues.Where(x => !string.IsNullOrEmpty(x.Category) && x.Category.ToLower() == "standard").ToList();
                List<Menu> macroMenues = allmenues.Where(x => !string.IsNullOrEmpty(x.Category) && x.Category.ToLower() == "macro").ToList();

                List<Menu> recommendedMenues = new List<Menu>();

                // 1st : try to fill with standard menu

                // 1. forloop standardMenues
                decimal? runningCal = 0;
                decimal? tempCal = 0;
                foreach (var menu in standardMenues) {
                    tempCal += menu.TotalCalories ;
                    if (tempCal <= cal + ConstAdvforStandard) {                       
                        recommendedMenues.Add(menu);
                        runningCal += menu.TotalCalories;
                    }
                    else
                    {
                        var recommendedstandardmenu = recommendedMenues.Where(x => x.Category.ToLower() == "standard").ToList();

                        var menuself = recommendedstandardmenu.Where(x => x.MenuName == menu.MenuName).ToList();

                        if (recommendedstandardmenu.Count > 0 && menuself.Count == 0)
                        {
                            for (int i= 0;i < recommendedstandardmenu.Count;i++)
                            {
                                if (tempCal - recommendedstandardmenu[i].TotalCalories  <= cal  /*tempCal - recommendedstandardmenu[i].TotalCalories <= cal*/)
                                {
                                    tempCal -= recommendedstandardmenu[i].TotalCalories;
                                    runningCal -= recommendedstandardmenu[i].TotalCalories;

                                    recommendedMenues.Remove(recommendedstandardmenu[i]);



                                    recommendedMenues.Add(menu);
                                    runningCal += menu.TotalCalories;
                                    break;
                                }                               
                            }
                            
                        }
                        else
                        {
                            if(tempCal <= cal)
                            {
                                recommendedMenues.Add(menu);
                                runningCal += menu.TotalCalories;
                            }
                        }
                        
                    }
                }

                // if fail to recommend in the 1st stage
                if(recommendedMenues.Count == 0)
                {
                    var defaultmenu = bitesByteDbContext.Menus.Where(x => x.Category == "standard").OrderBy(x => x.TotalCalories).FirstOrDefault();
                    recommendedMenues.Add(defaultmenu);
                    tempCal += defaultmenu.TotalCalories;
                    runningCal += defaultmenu.TotalCalories;


                }


                // 1. forloop macroMenues
                ;
                foreach (var macromenu in macroMenues)
                {
                    tempCal += macromenu.TotalCalories;
                    if (tempCal <= cal + ConstAdvforMacro)
                    {
                        recommendedMenues.Add(macromenu);
                    }
                    else
                    {
                        var recommendedmacromenu = recommendedMenues.Where(x => x.Category.ToLower() == "macro").ToList();

                        if (recommendedmacromenu.Count > 0)
                        {

                            for (int i = 0; i < recommendedmacromenu.Count; i++)
                            {
                                if (tempCal - recommendedmacromenu[i].TotalCalories <= cal)
                                {
                                    tempCal -= recommendedmacromenu[i].TotalCalories;
                                    recommendedMenues.Remove(recommendedmacromenu[i]);


                                    recommendedMenues.Add(macromenu);
                                }
                            }

                        }
                        else
                        {
                            if (tempCal <= cal + ConstAdvforMacro)
                            {
                                recommendedMenues.Add(macromenu);
                            }
                        }

                    }
                }

                

                return recommendedMenues;

            }
            catch (Exception) { throw; }
        }
    }
}
