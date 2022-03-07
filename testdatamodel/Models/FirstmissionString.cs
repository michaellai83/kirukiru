using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    public class FirstmissionString
    {
        /// <summary>
        /// 前置任務的內文ID
        /// </summary>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int Id { get; set; }
       
        /// <summary>
        /// 前置任務的內容(或者道具數量,單位)
        /// </summary>
        [Display(Name = "前置任務內容")]
        public string Main { get; set; }
        /// <summary>
        /// 創建時間
        /// </summary>
        [Display(Name = "創建時間")]
        public DateTime InitDate { get; set; }

        public virtual ICollection<Firstmission> Firstmissions { get; set; }
    }
}