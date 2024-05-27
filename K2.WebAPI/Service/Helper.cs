using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using static K2.WebAPI.Service.Helper;

namespace K2.WebAPI.Service
{
    public static class ApiExtension
    {
        public static HttpResponseMessage Response(this HttpRequestMessage request, ApiStatus statusCode, object dataValue, string msg)
        {
            HttpStatusCode stcd = (HttpStatusCode)statusCode.StatusCode;
            if (statusCode.StatusCode != 200)
            {
            }
            else
            {

            }
            return request.CreateResponse(stcd, dataValue);

        }
    }
    public class Helper
    {
        public class ApiResult
        {
            public string status { get; set; }
            public string error { get; set; }
            public string detail { get; set; }
            public string message { get; set; }
            public object data { get; set; }
        }
      
        public class ApiStatus
        {
            public int StatusCode { get; private set; }

            public string StatusDescription { get; private set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string Message { get; private set; }

            public ApiStatus(int statusCode)
            {
                HttpStatusCode parsedCode;
                try
                {
                    parsedCode = (HttpStatusCode)statusCode;
                    this.StatusCode = statusCode;
                    this.StatusDescription = parsedCode.ToString();
                }
                catch (System.Exception)
                {
                    this.StatusCode = statusCode;
                }
                
            }

            public ApiStatus(int statusCode, string statusDescription)
            {
                this.StatusCode = statusCode;
                this.StatusDescription = statusDescription;
            }

            public ApiStatus(int statusCode, string statusDescription, string message)
                : this(statusCode, statusDescription)
            {
                this.Message = message;
            }
        }


    }
}