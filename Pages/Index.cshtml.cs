using Catologs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Catologs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogs _catalogs;
        public IndexModel(ICatalogs catalogs)
        {
            _catalogs = catalogs;
        }


        public List<CatalogMain> catalogMains;
        public void OnGet()
        {
            catalogMains = _catalogs.GetCatalogMain();
        }
    }

}
