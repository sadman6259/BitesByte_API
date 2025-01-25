using BitesByte_API.Model;

namespace BitesByte_API.Service
{
    public interface IMenuService
    {
        List<Menu> InsertandRetrieveMenues(List<Menu> menus);
        List<Menu> GetAvailableMenu();
    }
    public class MenuService : IMenuService
    {
        private readonly BitesByteDbContext bitesByteDbContext;
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
    }
}
