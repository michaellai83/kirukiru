using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.Swagger
{
    /// <summary>
    /// 一般文章的編輯資訊
    /// </summary>
    public class NormalArticleOutPutForEdit
    {
        public bool success { get; set; }
        public ICollection<NormalData> data { get; set; }
        public class NormalData
        {
            public int artId { get; set; }
            public string main { get; set; }
            public int articlecategoryId { get; set; }
            public DateTime artInitDate { get; set; }
            public bool isFree { get; set; }
            public bool isPush { get; set; }
        }
    }
}