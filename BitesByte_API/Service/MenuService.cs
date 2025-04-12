using BitesByte_API.Model;
using System.Runtime.CompilerServices;

namespace BitesByte_API.Service
{
    public interface IMenuService
    {
        List<Menu> InsertandRetrieveMenues(List<Menu> menus);
        public void UpdateMenues(List<Menu> menus);
        List<Menu> GetAvailableMenu();
        List<Menu> GetMenusBySubCategory(string subcat,string category);
        List<Menu> GetRecommendedMenuByCalorie(decimal? cal);
        List<Menu> GetRecommendedMenuByProteinCarbs(decimal? protein,decimal? carbs);

    }
    public class MenuService : IMenuService
    {
        private readonly BitesByteDbContext bitesByteDbContext;

        private readonly int ConstAdvforStandard = 30;

        //private readonly int ConstAdvforMacro = 10;

        private readonly decimal ConstAdvforMacro1stprio = 0.5M;
        private readonly decimal ConstAdvforMacro2ndprio = 0.25M;
        public MenuService(BitesByteDbContext _bitesByteDbContext) { 
          this.bitesByteDbContext = _bitesByteDbContext;
        }
        public void UpdateMenues(List<Menu> menus)
        {
            try {
                foreach (var menu in menus) { 
                   Menu menuDB = bitesByteDbContext.Menus.FirstOrDefault(x => x.MenuName == menu.MenuName);
  
                    menuDB.Image = menu.Image;
                }
                bitesByteDbContext.SaveChanges();
            }
            catch (Exception) { throw; }
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

        public List<Menu> GetMenusBySubCategory(string subcat, string category)
        {
            try
            {
                List<Menu> menus = new List<Menu>();
                menus = bitesByteDbContext.Menus.Where(x => x.Subcategories.Contains(subcat) && category == category).ToList();
                return menus;
            }
            catch (Exception) { throw; }
        }

        public List<Menu> GetAvailableMenu()
        {
            try
            {
                return bitesByteDbContext.Menus.ToList();

            }
            catch (Exception) { throw;  }
        }

        public List<Menu> GetRecommendedMenuByCalorie(decimal? cal)
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

                        // if the current menu is not aldy added as a recommended menu
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
                    if (tempCal <= cal)
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
                            if (tempCal <= cal )
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

        public List<Menu> GetRecommendedMenuByProteinCarbs(decimal? protein, decimal? carbs)
        {
            List<Menu> allmenues = bitesByteDbContext.Menus.ToList();
            List<Menu> macroMenues = allmenues.Where(x => !string.IsNullOrEmpty(x.Category) && x.Category.ToLower() == "macro").ToList();

            List<Menu> proteinMacroMenues = macroMenues.Where(x => x.Protein != null && x.Protein > 0 && x.Protein < protein && x.Subcategories.ToLower() == "protein").OrderBy(x => x.Protein).ToList();
            List<Menu> carbsMacroMenues = macroMenues.Where(x => x.Carbs != null && x.Carbs > 0 && x.Carbs < carbs && x.Subcategories.ToLower() == "carbs").OrderBy(x => x.Carbs).ToList();

            List<Menu> recommendedProteinMenues = new List<Menu>();
            List<Menu> recommendedCarbsMenues = new List<Menu>();

            decimal? runningProtein = 0;
            decimal? runningCarbs = 0;


            // 1st prio: check if there is any micro menu which is 
            // +- 5g protein/carbs from input
            Menu proteinMenu1stPrio = new Menu();
            if (protein != null && protein > 0)
            {
                proteinMenu1stPrio = proteinMacroMenues.FirstOrDefault(x => x.Protein >= protein - ConstAdvforMacro1stprio && x.Protein <= protein );
                if(proteinMenu1stPrio != null && !string.IsNullOrWhiteSpace(proteinMenu1stPrio.MenuName))
                {
                    recommendedProteinMenues.Add(proteinMenu1stPrio);
                    runningProtein += proteinMenu1stPrio.Protein;
                }
            }

            Menu carbsMenu1stPrio = new Menu();
            if (carbs != null && carbs > 0)
            {
                carbsMenu1stPrio = carbsMacroMenues.FirstOrDefault(x => x.Carbs >= carbs - ConstAdvforMacro1stprio && x.Carbs <= carbs);
                if (carbsMenu1stPrio != null && !string.IsNullOrWhiteSpace(carbsMenu1stPrio.MenuName))
                {
                    recommendedCarbsMenues.Add(carbsMenu1stPrio);
                    runningCarbs += carbsMenu1stPrio.Carbs;
                }
            }


            // 2nd prio: if menu list is still null , then check if there is any food
            // which is exactly 0.5 +- 2.5 of the input

            if (recommendedProteinMenues.Count == 0 && runningProtein == 0 )
            {
                Menu proteinMenu2ndPrio = new Menu();
                proteinMenu2ndPrio = proteinMacroMenues.FirstOrDefault(x => x.Protein >= ((protein / 2) - ConstAdvforMacro2ndprio) && x.Protein <= (protein / 2));
                if (proteinMenu2ndPrio != null && !string.IsNullOrWhiteSpace(proteinMenu2ndPrio.MenuName))
                {
                    recommendedProteinMenues.Add(proteinMenu2ndPrio);
                    recommendedProteinMenues.Add(proteinMenu2ndPrio);
                    runningProtein += (proteinMenu2ndPrio.Protein * 2);
                }
            }

            if (recommendedCarbsMenues.Count == 0 && runningCarbs == 0)
            {
                Menu carbsMenu2ndPrio = new Menu();
                carbsMenu2ndPrio = carbsMacroMenues.FirstOrDefault(x => x.Carbs >= ((carbs / 2) - ConstAdvforMacro2ndprio) && x.Carbs <= (carbs / 2));
                if (carbsMenu2ndPrio != null && !string.IsNullOrWhiteSpace(carbsMenu2ndPrio.MenuName))
                {
                    recommendedCarbsMenues.Add(carbsMenu2ndPrio);
                    recommendedCarbsMenues.Add(carbsMenu2ndPrio);
                    runningCarbs += (carbsMenu2ndPrio.Carbs * 2);
                }
            }

            // 3rd prio: if still protein/carbs not filled, then start taking from 
            // the least protein/carbs one to the most one. keep adding item untill 
            // the sum meets input. 

            if (protein != null && protein > 0 && runningProtein == 0 && recommendedProteinMenues.Count == 0 )
            {
                foreach (var menu in proteinMacroMenues)
                {
                    runningProtein += menu.Protein;
                    if (runningProtein <= protein)
                    {
                        recommendedProteinMenues.Add(menu);
                    }
                    else
                    {
                        // 4th prio: when 3rd step can't add any more items then it will come here.
                        // try replacing the current menue with the last added menu , if cond. meets
                        // then replace. if cond. doesnn't meet then compare with the second last ,
                        // like this go untill the first menue.

                        for(int i = recommendedProteinMenues.Count - 1; i >= 0; i--)
                        {
                            // removed last item protein
                            runningProtein -= recommendedProteinMenues[i].Protein;
                            //runningProtein += menu.Protein;

                            if (runningProtein <= protein)
                            {
                                // replace last item with new item
                                recommendedProteinMenues.Remove(recommendedProteinMenues[i]);
                                recommendedProteinMenues.Add(menu);
                                break;
                            }
                            else
                            {
                                // go back to orig. state
                                runningProtein -= menu.Protein;
                                runningProtein += recommendedProteinMenues[i].Protein;
                            }
                        }
                        

                    }
                }
            }

            if (carbs != null && carbs > 0 && runningCarbs == 0 && recommendedCarbsMenues.Count == 0 )
            {
                foreach (var menu in carbsMacroMenues)
                {
                    runningCarbs += menu.Carbs;
                    if (runningCarbs <= carbs)
                    {
                        recommendedCarbsMenues.Add(menu);
                    }
                    else
                    {
                        // 4th prio: when 3rd step can't add any more items then it will come here.
                        // try replacing the current menue with the last added menu , if cond. meets
                        // then replace. if cond. doesnn't meet then compare with the second last ,
                        // like this go untill the first menue.

                        for (int i = recommendedCarbsMenues.Count - 1; i >= 0; i--)
                        {
                            // removed last item protein
                            runningCarbs -= recommendedCarbsMenues[i].Carbs;
                            //runningCarbs += menu.Carbs;

                            if (runningCarbs <= carbs)
                            {
                                // replace last item with new item
                                recommendedCarbsMenues.Remove(recommendedCarbsMenues[i]);
                                recommendedCarbsMenues.Add(menu);
                                break;
                            }
                            else
                            {
                                // go back to orig. state
                                runningCarbs -= menu.Carbs;
                                runningCarbs += recommendedCarbsMenues[i].Carbs;
                            }
                        }


                    }
                }

            }


            List<Menu> recommendedFinalMenuList = recommendedProteinMenues.Union(recommendedCarbsMenues).ToList();

            return recommendedFinalMenuList;
        }
    }
}
