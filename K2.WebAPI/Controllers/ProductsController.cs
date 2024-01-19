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
using System.Web.Http;

namespace K2.WebAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly RestClient _client;
        public ProductsController()
        {
            _client= new RestClient("https://ecm-uat-dms.eigerindo.co.id/");
        }
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route ("GetToken")]
        public IHttpActionResult GetToken()
        {

            Models.ProductModel.Body modelsBody = new Models.ProductModel.Body();
            modelsBody.username = "testecm02";
            modelsBody.password = "Ecm0234#";
            modelsBody.tenantid = "1";

            try
            {

                //var client = new RestClient("https://ecm-uat-dms.eigerindo.co.id/DocufloRESTfulAPI-V2/rest/session");
                var request = new RestRequest("DocufloRESTfulAPI-V2/rest/session");

                request.Method = Method.POST;
                request.JsonSerializer.ContentType = "application/json";
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Length", "754");
                request.AddHeader("Host", "ecm-uat-dms.eigerindo.co.id");
                //request.AddHeader("Content-Type", "application/json");
                request.AddHeader("ChannelName", "Docuflo7");
                request.AddHeader("ChannelToken", "");

                request.AddParameter("application/json; charset=utf-8",JsonConvert.SerializeObject(modelsBody),ParameterType.RequestBody);

                //request.AddBody("username", "testecm02");
                //request.AddBody("password", "Ecm0234#");
                //request.AddBody("tenantid", "1");

                var response = _client.ExecuteAsPost(request,"post");

                var data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(response.Content);

                return Json(data);
                //var response = client.ExecuteAsPost(request, "Get");
            }
            catch (Exception ex)
            {
                //throw ex;
                var result = new
                {
                    InfoMessage = ex.Message,
                    ProcessSuccess = false,
                };
                return Json(result);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult GetProductByID(int id)
        {
            try
            {
                var client = new RestClient("https://testapi.jasonwatmore.com");
                var request = new RestRequest(string.Format("products/{0}", id));
                var response = client.ExecuteAsGet(request, "Get");

                // deserialize json string response to JsonNode object
                var data = System.Text.Json.JsonSerializer.Deserialize<JsonNode>(response.Content);

                var result = new
                {
                    ProcessSuccess = true,
                    id = data["id"].ToString(),
                    name = data["name"].ToString(),
                };

                return Json(result);

                // output result
                //Console.WriteLine($"----------------json properties----------------id: { data["id"]}name: { data["name"]} ----------------raw json data ----------------{ data} ");
                //return $"----------------json properties----------------id: { data["id"]}name: { data["name"]} ----------------raw json data ----------------{ data} ";

            }
            catch (Exception ex)
            {
                var result = new
                {
                    InfoMessage = ex.Message,
                    ProcessSuccess = false,
                };
                return Json(result);
            }
        }


    }
}