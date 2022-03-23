using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace testdatamodel.Models
{
    
    public class Member
    {
        [Key]//主鍵 PK
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int ID { get; set; }
        [Required]
        [MaxLength(200)]
        public string UserName { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Required]
        public string PasswordSalt { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        
        public int ArticlecategoryId { get; set; }

       

        public bool Isidentify { get; set; }

        public string Emailidentify { get; set; }
        public DateTime initDate { get; set; }

       public string PicName { get; set; }
       public  string FileName { get; set; }
       
       public string Introduction { get; set; }
       
       public bool Opencollectarticles { get; set; }


       public virtual ICollection<Article> Articles { get; set; }

       public virtual ICollection<ArticleNormal> ArticleNormals { get; set; }
       public virtual ICollection<Orderlist> Orderlists { get; set; }
       public virtual ICollection<Subscriptionplan> Subscriptionplans { get; set; }

    }
}