using System;
using System.Collections.Generic;
using System.Linq;
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
        public class Result
        { 
        }
        public class Errors
        {
            public Boolean ProcessSuccess { get; set; }
            public string InfoMessage { get; set; }
        }
    }
}