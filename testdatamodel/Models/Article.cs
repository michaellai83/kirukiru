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
    /// 切切文章
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        [Key]//主鍵 PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
       
        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Display(Name = "使用者帳號")]
        public string UserName { get; set; }
        /// <summary>
        /// 封面圖片的ID
        /// </summary>
        //ForeignKey
        [Display(Name = "封面圖片ID")]
        public int FirstPicId { get; set; }

        [ForeignKey("FirstPicId")]
        public virtual FirstPic FirstPic { get; set; }
        /// <summary>
        /// 文章標題
        /// </summary>
        [Display(Name = "文章標題")]
        public string Title { get; set; }
        /// <summary>
        /// 是否付費
        /// </summary>
        [Display(Name = "是否付費")]
        public Boolean IsFree { get; set; }
        /// <summary>
        /// 文章簡介
        /// </summary>
        [Display(Name = "文章簡介")]
        public string Introduction { get; set; }
        /// <summary>
        /// 文章類型
        /// </summary>
        //ForeignKey
        [Display(Name = "文章類別ID")]
        public int ArticlecategoryId { get; set; }

        [ForeignKey("ArticlecategoryId")]
        public virtual Articlecategory Articlecategory { get; set; }
        /// <summary>
        /// 是否發布
        /// </summary>
        [Display(Name = "是否發布")]
        public Boolean IsPush { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        [Display(Name = "創建時間")]
        public DateTime InitDate { get; set; }

        public virtual ICollection<Firstmission> Firstmissions { get; set; }
        public virtual ICollection<ArticleMain> ArticleMains { get; set; }
        
       
        public virtual ICollection<Message> Messages{get;set;}

        public virtual ICollection<Remark> Remarks { get; set; }

        public virtual ICollection<Member> Members { get; set; }

    }
}