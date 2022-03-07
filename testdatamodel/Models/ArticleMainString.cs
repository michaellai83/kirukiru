using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    public class ArticleMainString
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 切切內容的描述
        /// </summary>
        [Display(Name = "切切步驟的內容")]
        public string Main { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        [Display(Name = "創立時間")]
        public DateTime InDateTime { get; set; }
        public virtual ICollection<ArticleMain> ArticleMains { get; set; }
    }
}