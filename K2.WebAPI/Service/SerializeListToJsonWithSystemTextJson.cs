using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static K2.WebAPI.Models.ProductModel;

namespace K2.WebAPI.Service
{
    public class SerializeListToJsonWithSystemTextJson
    {
        private readonly List<Metadata> _MetadataList;
        public SerializeListToJsonWithSystemTextJson(List<Metadata> MetadataList)
        {
            _MetadataList = MetadataList;
        }
    }
}