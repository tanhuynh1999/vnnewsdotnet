using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnnews.Models
{
    public class JsonNews
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string datecreate { get; set; }
        public int view { get; set; }
        public string img { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<bool> bin { get; set; }
        public int idus { get; set; }

        //User
        public string usname { get; set; }
        public string usrole { get; set; }
    }
}