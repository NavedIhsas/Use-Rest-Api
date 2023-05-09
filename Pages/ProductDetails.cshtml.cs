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

        public ItemRoot ItemRoot;
        public void OnGet(string id,string catalogId)
        {
            ItemRoot = _catalog.GetCatalogDetails(id, catalogId);
        }
    }
}
