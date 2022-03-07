using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    /// <summary>
    /// 封面圖
    /// </summary>
    public class FirstPic
    {
        /// <summary>
        /// 封面ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }

        /// <summary>
        /// 封面圖片名稱
        /// </summary>
        [Display(Name = "封面圖片名稱")]
        public string Name { get; set; }
        /// <summary>
        /// 封面圖片副檔名
        /// </summary>
        [Display(Name="封面圖片的副檔名")]
        public string FileName { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}