using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string UserName { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Required]
        public string PasswordSalt { get; set; }
        [Required]
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        
        public int ArticlecategoryId { get; set; }

       

        public bool Isidentify { get; set; }

        public string Emailidentify { get; set; }
        public DateTime initDate { get; set; }

       public string PicName { get; set; }
       public  string FileName { get; set; }


       public virtual ICollection<Article> Articles { get; set; }

       public virtual ICollection<ArticleNormal> ArticleNormals { get; set; }

    }
}