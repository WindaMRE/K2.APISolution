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

        public class Errors
        {
            public Boolean ProcessSuccess { get; set; }
            public string InfoMessage { get; set; }
        }

        //public class D
        //{
        //    public List<GenerateSignatures> results { get; set; }
        //}

        public class GenerateSignatures
        {
            public List<ResultGen> Results { get; set; }
            public int Id { get; set; }
            public string Exception { get; set; }
            public string Status { get; set; }
            public bool IsCanceled { get; set; }
            public bool IsCompleted { get; set; }
            public int CreationOptions { get; set; }
            public string AsyncState { get; set; }
            public bool IsFaulted { get; set; }
        }

        public class ResultGen
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }
        //public partial class Root 
        //{
        //    public D d { get; set; }
        //}

        public class ProfileFieldDMS
        {
            public string FieldName { get; set; }
            public string Descriptions { get; set; }
            public string Values { get; set; }
        }
    }
}