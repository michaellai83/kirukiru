using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace testdatamodel.PutData
{
    public class DataArticle
    {
        public int ArtID { get; set; }
        public string Art_Title { get; set; }
        public string Art_TitlePic { get; set; }
        public string Art_Info { get; set; }
        public int Art_ArtlogId { get; set; }
        public ICollection <FirstM> Art_firstmission { get; set; }

        public ICollection <Main> Art_main { get; set; }
        
        public  string Art_Isfree { get; set; }
        public string Art_Ispush { get; set; }
        public class  FirstM 
        {
            public int FirstID { get; set; }
            public string FirstPic { get; set; }
            public string FirstMain { get; set; }
            
            public virtual List<DataArticle> DataArticles { get; set; }
        }
        public class Main
        {
            public int MainID { get; set; }
            public string MainPic { get; set; }
            public string MainStr { get; set; }
            public virtual List<DataArticle> DataArticles { get; set; }
        }
    }
}