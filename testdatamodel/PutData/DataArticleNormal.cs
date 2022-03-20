using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.PutData
{
    public class DataArticleNormal
    {
        public string userName { get; set; }
        public string title { get; set; }
        public string main { get; set; }
        public int articlecategoryId { get; set; }
        public bool isFree { get; set; }
        public bool isPush { get; set; }
    }
}