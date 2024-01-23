using K2.WebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static K2.WebAPI.Models.ProductModel;
using static K2.WebAPI.Service.Helper;
//using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
//using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
//using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace K2.WebAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly ProductService _prodserv;
        public ProductsController() => _prodserv = new ProductService();

        [HttpPost]
        [Route("GetToken")]
        public IHttpActionResult GetToken(string Username, string Password)
        {

            Models.ProductModel.Body modelsBody = new()
            {
                username = Username,//"testecm02",
                password = Password,//"Ecm0234#",
                tenantid = "1"
            };

            try
            {

                //var client = new RestClient("https://ecm-uat-dms.eigerindo.co.id/DocufloRESTfulAPI-V2/rest/session");
                RestRequest request = new("DocufloRESTfulAPI-V2/rest/session")
                {
                    Method = Method.POST,
                };
                request.JsonSerializer.ContentType = "application/json";
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Length", "754");
                request.AddHeader("Host", "ecm-uat-dms.eigerindo.co.id");
                //request.AddHeader("Content-Type", "application/json");
                request.AddHeader("ChannelName", "Docuflo7");
                request.AddHeader("ChannelToken", "");
                request.AddParameter("application/json; charset=utf-8", Newtonsoft.Json.JsonConvert.SerializeObject(modelsBody), ParameterType.RequestBody);
                return null;
                // return Json(_prodserv.ServiceGET);

            }
            catch (Exception ex)
            {
                var result = new
                {
                    InfoMessage = ex.Message,
                    ProcessSuccess = false,
                };
                return null;// Json(result);
            }
        }

        //[Authorize]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Metadata>))]
        [Route("GetSAPLogisticStore")]
        public async Task<HttpResponseMessage> GetSAPLogisticStoreDn(string UserName, string Password)
        {
            try
            {
                //bool Autorize= Authorize.GetAuthorize(UserName, Password);
                //if (Autorize == true)
                //{
                RestClient client = new("https://tracking.pos-eigerindo.com/saplogistics/storedn.php?store_code=2002");
                RestRequest request = new()
                {
                    Method = Method.GET
                };
                request.JsonSerializer.ContentType = "application/json";
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Length", "754");
                request.AddHeader("Host", "tracking.pos-eigerindo.com");
                request.AddHeader("Authorization", "Basic c3VuYXJkeUBlaWdlcmluZG8uY28uaWQKOkVpZzNSTDBn");

                
                var data = _prodserv.ServiceGET<Root>(request);
                
                var StoreToDNNavResult = data.d.results[0].StoreToDNNav.results;

                var testdataList = new List<Metadata>();

                foreach (var item in StoreToDNNavResult)
                {
                    Metadata DataMetadata = new Metadata();
                    DataMetadata.id = item.__metadata.id;
                    DataMetadata.uri = item.__metadata.uri;
                    DataMetadata.type = item.__metadata.type;

                    testdataList.Add(DataMetadata);

                };
                var arr = testdataList.ToArray();

                return Request.Response(new ApiStatus(200), arr, "Success");
            }
            catch (Exception ex)
            {
                return Request.Response(new ApiStatus(500, ex.Message), null, "Error");
            }
        }

        //public IHttpActionResult GetProductByID(int id)
        //{
        //    try
        //    {
        //        var client = new RestClient("https://testapi.jasonwatmore.com");
        //        var request = new RestRequest(string.Format("products/{0}", id));
        //        var response = client.ExecuteAsGet(request, "Get");

        //        // deserialize json string response to JsonNode object
        //        var data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(response.Content);

        //        var result = new
        //        {
        //            ProcessSuccess = true,
        //            id = data["id"].ToString(),
        //            name = data["name"].ToString(),
        //        };

        //        return Json(result);

        //        // output result
        //        //Console.WriteLine($"----------------json properties----------------id: { data["id"]}name: { data["name"]} ----------------raw json data ----------------{ data} ");
        //        //return $"----------------json properties----------------id: { data["id"]}name: { data["name"]} ----------------raw json data ----------------{ data} ";

        //    }
        //    catch (Exception ex)
        //    {
        //        var result = new
        //        {
        //            InfoMessage = ex.Message,
        //            ProcessSuccess = false,
        //        };
        //        return Json(result);
        //    }
        //}


    }
}