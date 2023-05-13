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
        Root GetCatalogMain();
        List<CatalogPage> GetCatalogPage(string id, string nId,string rIn);
        ItemRoot GetCatalogDetails(string id, string catalogId, string rIn);
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

        public Root GetCatalogMain()
        {
            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogMain?token={token}", Method.Get);
            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var root= JsonConvert.DeserializeObject<Root>(restult.Content);
              
                return root;
            }
            else throw new Exception(restult.ErrorMessage);
        }

        public List<CatalogPage> GetCatalogPage(string id, string nId,string rIn)
        {
            if (string.IsNullOrEmpty(nId))
                return null;

            var gId = "{4C7FD04E-7716-44F5-BA98-0353A5D9DAFC}";

            if (nId != gId)
            {
                var main = GetCatalogMain().CatalogMain.Any(x => x.CatalogNoId == nId);
                if (!main)
                    return null;
            }
           

            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogPage?catalogId={id}&token={token}", Method.Get);
            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var application = JsonConvert.DeserializeObject<Application>(restult.Content).CatalogPage;
                
                var list = new List<CatalogPage>();
                foreach (var item in application)
                {
                    var image1 = Convert.FromBase64String(item.Image);
                    item.ImageByte = image1;
                    item.RemainInvertory = rIn;
                    list.Add(item);
                }
                return list;
            }
            else throw new Exception(restult.ErrorMessage);
        }

        public ItemRoot GetCatalogDetails(string id, string catalogId,string rIn)
        {
            var token = _configuraiton["BaseUrl:token"];
            var request = new RestRequest($"/crm/GetCatalogPageItem?catalogPageId={id}&token={token}", Method.Get);

            var gId = "{4C7FD04E-7716-44F5-BA98-0353A5D9DAFC}";
            var catalogPage = GetCatalogPage(catalogId,gId, rIn).FirstOrDefault(x => x.PageId.Equals(id));

            var restult = _catalog.Execute(request);
            if (restult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var details = JsonConvert.DeserializeObject<ItemRoot>(restult.Content);
                details.Order = catalogPage.Order;
                details.Desc = catalogPage.Desc;
                details.Name = catalogPage.Name;
                return details;
            }
            else throw new Exception(restult.ErrorMessage);
        }
    }
}
