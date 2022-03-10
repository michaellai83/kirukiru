using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    /// <summary>
    /// 後台管理員
    /// </summary>
    public class Backmember
    {
        [Key]//主鍵 PK
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        [Display(Name = "username")]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime IniDateTime { get; set; }

        public virtual ICollection<BackArticle> BackArticles { get; set; }
    }
}