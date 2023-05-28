using Catologs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catologs.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICatalogs _catalogs;
        public IndexModel(ICatalogs catalogs)
        {
            _catalogs = catalogs;
        }


        public Root CatalogMain;
        public void OnGet()
        {
            CatalogMain = _catalogs.GetCatalogMain();

        }
    }

}
