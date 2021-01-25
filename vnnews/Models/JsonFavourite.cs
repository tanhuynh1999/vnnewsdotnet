using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnnews.Models
{
    public class JsonFavourite
    {
        public int id { get; set; }
        public string datecreate { get; set; }
        public int idus { get; set; }
        public int idnews { get; set; }
    }
}