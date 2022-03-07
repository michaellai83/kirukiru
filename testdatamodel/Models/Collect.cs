using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    /// <summary>
    /// 收藏的資料表
    /// </summary>
    public class Collect
    {
        /// <summary>
        /// 收藏的ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 會員的帳號
        /// </summary>
        [Display(Name ="會員帳號")]
        public string UserName { get; set; }
        [Display(Name = "切切文章ID")]
        public int ArticleId { get; set; }

        
        ///// <summary>
        ///// 收藏的一般文章ID
        ///// </summary>
        //[Display(Name = "一般文章ID")]
        //public int ArticleNorId { get; set; }

       

    }
}