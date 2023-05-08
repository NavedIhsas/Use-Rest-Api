using Catologs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catologs.Pages
{
    public class ProductDetailsModel : PageModel
    {

        private readonly ICatalogs _catalog;
        public ProductDetailsModel(ICatalogs catalogs)
        {
            _catalog = catalogs;
        }

        public List<CatalogPageItem> CatalogItems;
        public void OnGet(string id)
        {
           CatalogItems= _catalog.GetCatalogDetails(id);
        }
    }
}
