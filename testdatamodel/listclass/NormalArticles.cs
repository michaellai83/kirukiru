using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 正常文章列表
    /// </summary>
    public class NormalArticles
    {
        public int ArticleID { get; set; }
        public string UserName { get; set; }
        public string Introduction { get; set; }
        public string Title { get; set; }
        public string Articlecategory { get; set; }
        public bool Isfree { get; set; }
        public int Lovecount { get; set; }
        public DateTime InitDateTime { get; set; }
    }
}