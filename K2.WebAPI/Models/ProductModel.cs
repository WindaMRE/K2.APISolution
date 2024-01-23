using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Web;

namespace K2.WebAPI.Models
{
    public partial class ProductModel
    {
        public class Parameter
        { 
        }

        public class Body
        {
            public string username { get; set; }
            public string password { get; set; }
            public string tenantid { get; set; }
        }
       
        public class Errors
        {
            public Boolean ProcessSuccess { get; set; }
            public string InfoMessage { get; set; }
        }

        public class D
        {
            public List<Result> results { get; set; }
        }

        public class Metadata
        {
            public string id { get; set; }
            public string uri { get; set; }
            public string type { get; set; }
        }
        public class metadata2
        {
            public List<string> id { get; set; }
            public List<string> uri { get; set; }
            public List<string> types { get; set; }
        }
        //public class Mid
        //{
        //    public string id { get; set; }
        //}
        //public class Muri
        //{
        //    public string uri { get; set; }
        //}
        //public class Mtype
        //{
        //    public string type { get; set; }
        //}
        //public class metadata2
        //{
        //    public string Content { get; set; }
        //}
        public class Result
        {
            public Metadata __metadata { get; set; }
            public string store_code { get; set; }
            public StoreToDNNav StoreToDNNav { get; set; }
            public string dn_number { get; set; }
        }

        public partial class Root 
        {
            public D d { get; set; }
        }

        public class StoreToDNNav
        {
            public List<Result> results { get; set; }
        }
    }
}