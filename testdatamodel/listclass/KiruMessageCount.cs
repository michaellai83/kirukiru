using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 切切文章有留言數量
    /// </summary>
    public class KiruMessageCount
    {
        public int artId { get; set; }

        public string author { get; set; }
        public string authorPic { get; set; }
        public string username { get; set; }
        public string title { get; set; }
        public string introduction { get; set; }
        public int articlecategoryId { get; set; }
        public string artArtlog { get; set; }
        public string firstPhoto { get; set; }
        public bool isFree { get; set; }
        public int lovecount { get; set; }
        public int messageCount { get; set; }
        public DateTime artInitDate { get; set; }
    }
}