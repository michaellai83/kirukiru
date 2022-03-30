using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    /// <summary>
    /// 後台文章
    /// </summary>
    public class BackArticle
    {
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        [Display(Name = "後台管理員ID")]
        public int BackmemberID { get; set; }
        [ForeignKey("BackmemberID")]
        public virtual Backmember Backmembers  { get; set; }
        public string BackMemberPic { get; set; }
        public string Title { get; set; }
        public string Titlepic { get; set; }
        public string Main { get; set; }

        public DateTime IniDateTime { get; set; }
    }
}