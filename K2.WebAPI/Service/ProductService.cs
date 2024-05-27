using Dapper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Xml;
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
        //public Root ServiceGET<T>(RestRequest request) where T : Root
        //{
        //    try
        //    {
        //        return System.Text.Json.JsonSerializer.Deserialize<T>(_clientGet.ExecuteAsGet(request, "get").Content);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public GenerateSignatures ServicePOST<T>(IRestRequest request) where T : GenerateSignatures
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


        public string GetSignatureDB(int WorkflowID,string Activity)
        {
            try
            {
                Repository rp = new Repository();
                DynamicParameters dParam = new DynamicParameters();
                dParam.Add("@WorkflowId", WorkflowID, DbType.Int64, ParameterDirection.Input);
                dParam.Add("@Activity", Activity, DbType.String, ParameterDirection.Input);

                string InsertData = (rp.DynamicParamQuery<string>("dbo.[precise.05.SP_GetSignature]", dParam, null, true, CommandType.StoredProcedure)).List.FirstOrDefault();

                return InsertData;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
   
}