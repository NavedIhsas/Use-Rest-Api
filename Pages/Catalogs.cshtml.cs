using Catologs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Catologs.Pages
{
    public class SliderModel : PageModel
    {
        private readonly ICatalogs _cotalog;
        public SliderModel(ICatalogs cotalog)
        {
            _cotalog = cotalog;
        }

        public List<CatalogPage> catalogPages;
        public  IActionResult OnGet(string id,string nId)
        {
           catalogPages= _cotalog.GetCatalogPage(id,nId);
            if (catalogPages == null)
              return  RedirectToPage("/Error");

            return Page();
        }
    }
}
