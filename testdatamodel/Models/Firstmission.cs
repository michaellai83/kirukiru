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
    /// 前置任務
    /// </summary>
    public class Firstmission
    {
        /// <summary>
        /// 前置任務ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
        /// <summary>
        /// 前置任務的圖片名稱
        /// </summary>
        [Display(Name ="圖片名稱")]
        public string PicName { get; set; }
        /// <summary>
        /// 前置任務圖片的副檔名
        /// </summary>
        [Display(Name ="圖片副檔名")]
        public string PicFileName { get; set; }
        [Display(Name = "內文ID")]
        public int FirstmissionStringId { get; set; }

        [ForeignKey("FirstmissionStringId")]
        public virtual FirstmissionString FirstmissionStrings { get; set; }

        /// <summary>
        /// 文章的ID
        /// </summary>
        [Display(Name = "文章ID")]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Articles { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        [Display(Name = "創建時間")]
        public DateTime InitDate { get; set; }

    }
}