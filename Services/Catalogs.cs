using Newtonsoft.Json;
using RestSharp;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Catologs.Services
{
    public interface ICatalogs
    {
        List<CatalogMain> GetCatalogMain();
        List<CatalogPage> GetCatalogPage(string id);
        ItemRoot GetCatalogDetails(string id, string catalogId);
    }
    public class Catalog : ICatalogs
    {
        private readonly RestClient _catalog;
        private readonly IConfiguration _configuraiton;
        public Catalog(RestClient catalogs, IConfiguration configuraiton)
        {
            _catalog = catalogs;
            _configuraiton = configuraiton;
        }

        public List<CatalogMain> GetCatalogMain()
        {
            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogMain?token={token}", Method.Get);
            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Root>(restult.Content).CatalogMain;
            else throw new Exception(restult.ErrorMessage);
        }

        public List<CatalogPage> GetCatalogPage(string id)
        {
            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogPage?catalogId={id}&token={token}", Method.Get);
            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var catalog = JsonConvert.DeserializeObject<Application>(restult.Content).CatalogPage;
                var list = new List<CatalogPage>();
                foreach (var item in catalog)
                {
                    var image1 = Convert.FromBase64String(item.Image);
                    item.ImageByte = image1;
                    list.Add(item);
                }
                return list;
            }
            else throw new Exception(restult.ErrorMessage);
        } 
        
        public ItemRoot GetCatalogDetails(string id,string catalogId)
        {
            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogPageItem?catalogPageId={id}&token={token}", Method.Get);

            var catalogPage = GetCatalogPage(catalogId).FirstOrDefault(x=>x.PageId.Equals(id));
        
            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var details= JsonConvert.DeserializeObject<ItemRoot>(restult.Content);
                details.Order = catalogPage.Order;
                details.Desc=catalogPage.Desc;
                details.Name = catalogPage.Name;
                return details;
            }
            else throw new Exception(restult.ErrorMessage);
        }
    }
}
