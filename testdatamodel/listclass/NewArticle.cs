﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 最新文章列表
    /// </summary>
    public class NewArticle
    {
        public int ArticleID { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string ArtInfo { get; set; }
        public string Articlecategory { get; set; }
        public string ArtPic { get; set; }
        public bool Isfree { get; set; }
        public int Lovecount { get; set; }
        public DateTime InitDateTime { get; set; }
    }
}