using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 留言格式
    /// </summary>
    public class MessageList
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        public int messageId { get; set; }
        /// <summary>
        /// 留言帳號
        /// </summary>
        public string messageUserName { get; set; }
        /// <summary>
        /// 留言名字
        /// </summary>
        public string messageMember { get; set; }
        /// <summary>
        /// 留言者圖片
        /// </summary>
        public string messageMemberPic { get; set; }
        /// <summary>
        /// 留言內容
        /// </summary>
        public string messageMain { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        public DateTime messageInitDate { get; set; }
        /// <summary>
        /// 回覆留言
        /// </summary>
        public List<RMG> reMessageData { get; set; }

        public class RMG
        {
            /// <summary>
            /// 回覆留言ID
            /// </summary>
            public int reMessageId { get; set; }
            /// <summary>
            /// 作者名字
            /// </summary>
            public string author { get; set; }
            /// <summary>
            /// 作者圖片
            /// </summary>
            
            public string authorPic { get; set; }
            /// <summary>
            /// 回覆留言內容
            /// </summary>
            public string reMessageMain { get; set; }
            /// <summary>
            /// 回覆留言時間
            /// </summary>
            public DateTime reMessageInitDate { get; set; }
        }
    }
}