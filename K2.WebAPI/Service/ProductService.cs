using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using static K2.WebAPI.Models.ProductModel;

namespace K2.WebAPI.Service
{
   
    public partial class ProductService
    {
        //protected readonly RestClient _client;
        protected readonly RestClient _clientGet;
        protected readonly RestClient _clientPost;
        public ProductService()
        {
           // _client = new RestClient("https://tracking.pos-eigerindo.com/saplogistics/storedn.php?store_code=2002");
            _clientGet = new RestClient("https://tracking.pos-eigerindo.com/saplogistics/storedn.php?store_code=2002");
            _clientPost = new RestClient("https://ecm-uat-dms.eigerindo.co.id/");
        }
        public Root ServiceGET<T>(RestRequest request) where T : Root
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(_clientGet.ExecuteAsGet(request, "get").Content);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JsonNode ServicePOST<T>(RestRequest request) where T : JsonNode
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(_clientPost.ExecuteAsPost(request, "Post").Content);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }
   
}