using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    /// <summary>
    /// 訂閱名單
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// 訂閱的編號
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 訂閱人的帳號
        /// </summary>
        [Display(Name ="訂閱者的帳號")]
        public string UserName { get; set; }
        /// <summary>
        /// 回傳 以訂閱
        /// </summary>
        [Display(Name ="是否訂閱")]
        public string IsSub { get; set; }
        /// <summary>
        /// 被訂閱者的帳號
        /// </summary>
        [Display(Name ="被訂閱者的帳號")]
        public string Name { get; set; }
    }
}