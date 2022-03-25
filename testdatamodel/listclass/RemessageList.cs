using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 回覆留言
    /// </summary>
    public class RemessageList
    {
        public int reMessageId { get; set; }
        public string reMessageMain { get; set; }
        public DateTime reMessageInitDate { get; set; }
    }
}