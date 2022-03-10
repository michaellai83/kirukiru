using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.PutData
{
    public class DataArticleNormal
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Main { get; set; }
        public int ArticlecategoryId { get; set; }
        public bool IsFree { get; set; }
        public bool IsPush { get; set; }
    }
}