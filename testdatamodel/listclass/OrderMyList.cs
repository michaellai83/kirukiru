using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testdatamodel.listclass
{
    /// <summary>
    /// 我的訂閱清單
    /// </summary>
    public class OrderMyList
    {
        /// <summary>
        /// 清單ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 訂閱的作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 訂閱金額
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 交易是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 訂閱日期
        /// </summary>
        public DateTime InitDate { get; set; }
    }
}