using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.PutData
{
    /// <summary>
    /// 更改會員頭像
    /// </summary>
    public class ChangePhoto
    {
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 會員圖片
        /// </summary>
        public string userPhoto { get; set; }
    }

}