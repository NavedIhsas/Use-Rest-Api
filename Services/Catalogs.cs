using Newtonsoft.Json;
using RestSharp;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Catologs.Services
{
    public interface ICatalogs
    {

        Root GetCatalogMain();
        List<CatalogPage> GetCatalogPage(string id, string nId, string rIn);
        ItemRoot GetCatalogDetails(string id, string catalogId, string rIn);
    }
    public class Catalog : ICatalogs
    {
        private readonly RestClient _catalog;
        private readonly IConfiguration _configuraiton;

        private readonly ILogger<Catalog> _logger;
        public Catalog(RestClient catalogs, IConfiguration configuraiton, ILogger<Catalog> logger)
        {
            _catalog = catalogs;
            _configuraiton = configuraiton;
            _logger = logger;
        }
        public Catalog(RestClient catalog, IConfiguration configuraiton)
        {
            _catalog = catalog;
            _configuraiton = configuraiton;
        }

        public Root GetCatalogMain()
        {
            try
            {
                var token = _configuraiton["BaseUrl:token"];
                var request = new RestRequest($"/crm/GetCatalogMain?token={token}", Method.Get);
                var restult = _catalog.Execute(request);
                if (restult.StatusCode != HttpStatusCode.OK)
                {
                    var result = new Root()
                    {
                        Message = "NotFount",
                    };
                    _logger.LogWarning($"کاتالوگ ها دریافت نشد {restult.ErrorMessage} statusCode={restult.StatusCode}");
                    return result;
                }

                if (restult.Content != null)
                {
                    var root = JsonConvert.DeserializeObject<Root>(restult.Content);
                    return root;
                }
                _logger.LogWarning($" {restult.Content} و statusCode={restult.StatusCode} هیچ محتوای برای این کاتالوگ وجود ندارد");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"حین لود کردن سرویس GetCatalogMain() خطای زیر رخ داد {e}");
                throw new Exception(e.Message);
            }
        }

        public List<CatalogPage> GetCatalogPage(string id, string nId, string rIn)
        {
            try
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
                if (restult.StatusCode == HttpStatusCode.OK)
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
                _logger.LogError(restult.ErrorException,restult.ErrorMessage);
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"حین دریافت صفحه کاتالوگ خطای زیر رخ داد {e}"  );
                throw new Exception(e.Message);
            }
        }

        public ItemRoot GetCatalogDetails(string id, string catalogId, string rIn)
        {
           
            try
            {
                var token = _configuraiton["BaseUrl:token"];
                var request = new RestRequest($"/crm/GetCatalogPageItem?catalogPageId={id}&token={token}", Method.Get);

                var gId = "{4C7FD04E-7716-44F5-BA98-0353A5D9DAFC}";
                var catalogPage = GetCatalogPage(catalogId, gId, rIn).FirstOrDefault(x => x.PageId.Equals(id));

                var restult = _catalog.Execute(request);
                if (restult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var details = JsonConvert.DeserializeObject<ItemRoot>(restult.Content);
                    details.Order = catalogPage.Order;
                    details.Desc = catalogPage.Desc;
                    details.Name = catalogPage.Name;
                    return details;
                }
                _logger.LogError($"حین دریافت جزییات کاتالوگ با شناسه {id} خطای زیر رخ داده است {restult.ErrorException}");
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError($"حین دریافت جزییات کاتالوگ با شناسه {id} خطای زیر رخ داده است {e}");
                throw new Exception(e.Message);

            }
        }
    }
}
