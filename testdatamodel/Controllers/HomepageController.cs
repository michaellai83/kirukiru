using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using testdatamodel.listclass;
using testdatamodel.Models;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 首頁的API
    /// </summary>
    public class HomepageController : ApiController
    {
        ProjectDb db = new ProjectDb();

        /// <summary>
        /// 依類別取得一般文章和切切文章(按照時間最新排列
        /// </summary>
        /// <param name="artlog">類別的數字值</param>
        /// <param name="nowpage">現在的頁數(一開始請填1)</param>
        /// <param name="pagecount">總共幾筆資料(一開始請填0)</param>
        /// <param name="showcount">要顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekArticle(int artlog,int nowpage,int pagecount,int showcount)
        {
            var data = from a in db.Articles
                where (a.ArticlecategoryId == artlog &
                       a.IsPush == true)
                select a;

            var datanormal = from a in db.ArticleNormals
                where (a.ArticlecategoryId == artlog &
                       a.IsPush == true)
                select a;
            if (data != null && datanormal != null)
            {
                
                List<NewArticle> arrayList = new List<NewArticle>();

                if (pagecount == 0)
                {
                    foreach (var content in data.ToList())
                    {
                        //var good = db.Goods.Where(m => m.ArticleId == artid).Count();//不要在迴圈裡面開資料庫
                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                        newartary.ArtInfo = content.Introduction;
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);
                    }

                    foreach (var content in datanormal.ToList())
                    {

                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = "null";
                        newartary.ArtInfo = "null";
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);


                    }
                    //排序依照日期
                    //var result = from e in arrayList
                    //    orderby e.InitDateTime
                    //    select e;
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                    pagecount = arrayList.Count();
                    return Ok(new{datacount=pagecount, result});
                }
                else
                {
                    foreach (var content in data.ToList())
                    {
                        //var good = db.Goods.Where(m => m.ArticleId == artid).Count();//不要在迴圈裡面開資料庫
                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                        newartary.ArtInfo = content.Introduction;
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);
                    }

                    foreach (var content in datanormal.ToList())
                    {

                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = "null";
                        newartary.ArtInfo = "null";
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);


                    }

                    int page = (nowpage - 1) * showcount;
                    //排序依照日期
                    
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
                    return Ok(result);
                }
                
            }
            else
            {
                return NotFound();
            }
            
        }


        ///// <summary>
        ///// 依照時間尋找文章
        ///// </summary>
        ///// <param name="datetime">時間(EX:2022.03.08)</param>
        ///// <returns></returns>
        //[HttpGet]
        //public IHttpActionResult SeekTimeArticle(string datetime)
        //{
        //    try
        //    {
        //        ///當日搜尋
        //        var datetime1 = DateTime.Parse(datetime);
        //        var datetime2 = datetime1.AddDays(1);

        //        var data = from q in db.Articles
        //                   where q.InitDate >= datetime1 && q.InitDate < datetime2
        //                   select q;
        //        var dataNormal = from q in db.ArticleNormals
        //                         where q.InitDate >= datetime1 && q.InitDate < datetime2
        //                         select q;
        //        if (data != null && dataNormal != null)
        //        {
        //            List<NewArticle> arrayList = new List<NewArticle>();

        //            foreach (var content in data.ToList())
        //            {
        //                NewArticle newartary = new NewArticle();
        //                newartary.ArticleID = content.ID;
        //                newartary.UserName = content.UserName;
        //                newartary.Title = content.Title;
        //                newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
        //                newartary.ArtInfo = content.Introduction;
        //                newartary.Articlecategory = content.Articlecategory.Name;
        //                newartary.Isfree = content.IsFree;
        //                newartary.Lovecount = content.Lovecount;
        //                newartary.InitDateTime = content.InitDate;

        //                arrayList.Add(newartary);
        //            }

        //            foreach (var content in dataNormal.ToList())
        //            {

        //                NewArticle newartary = new NewArticle();
        //                newartary.ArticleID = content.ID;
        //                newartary.UserName = content.UserName;
        //                newartary.Title = content.Title;
        //                newartary.ArtPic = "null";
        //                newartary.ArtInfo = "null";
        //                newartary.Articlecategory = content.Articlecategory.Name;
        //                newartary.Isfree = content.IsFree;
        //                newartary.Lovecount = content.Lovecount;
        //                newartary.InitDateTime = content.InitDate;

        //                arrayList.Add(newartary);


        //            }
        //            //排序依照日期
        //            var result = arrayList.OrderByDescending(x => x.InitDateTime);
        //            //var resultArticles = from e in arrayList
        //            //    orderby e.InitDateTime
        //            //    select e;


        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 依照時間範圍搜尋文章
        /// </summary>
        /// <param name="datetime1">較早的時間(ex:2022.03.08)</param>
        /// <param name="datetime2">較晚的時間(ex:2022.03.10)</param>
        /// <param name="nowpage">現在頁數(一開始請直接傳1)</param>
        /// <param name="totalcount">總共有幾筆資料(一開始請填0)</param>
        /// <param name="showcount">顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekTimeArticle(string datetime1, string datetime2,int nowpage,int totalcount,int showcount )
        {
            try
            {
                var datetime01 = DateTime.Parse(datetime1);
                var datetime02 = DateTime.Parse(datetime2).AddDays(1);
                var data = from q in db.Articles
                    where (q.InitDate >= datetime01 && q.InitDate <datetime02 &q.IsPush==true)
                    select q;

                var dataNormal = from q in db.ArticleNormals
                    where q.InitDate >= datetime01 && q.InitDate <datetime02 & q.IsPush == true
                    select q;
                if (data != null && dataNormal != null)
                {
                    List<NewArticle> arrayList = new List<NewArticle>();

                    if (totalcount == 0)
                    {
                        foreach (var content in data.ToList())
                        {
                            NewArticle newartary = new NewArticle();
                            newartary.ArticleID = content.ID;
                            newartary.UserName = content.UserName;
                            newartary.Title = content.Title;
                            newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                            newartary.ArtInfo = content.Introduction;
                            newartary.Articlecategory = content.Articlecategory.Name;
                            newartary.Isfree = content.IsFree;
                            newartary.Lovecount = content.Lovecount;
                            newartary.InitDateTime = content.InitDate;

                            arrayList.Add(newartary);
                        }

                        foreach (var content in dataNormal.ToList())
                        {

                            NewArticle newartary = new NewArticle();
                            newartary.ArticleID = content.ID;
                            newartary.UserName = content.UserName;
                            newartary.Title = content.Title;
                            newartary.ArtPic = "null";
                            newartary.ArtInfo = "null";
                            newartary.Articlecategory = content.Articlecategory.Name;
                            newartary.Isfree = content.IsFree;
                            newartary.Lovecount = content.Lovecount;
                            newartary.InitDateTime = content.InitDate;

                            arrayList.Add(newartary);


                        }
                        //排序依照日期
                        var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                        //var resultArticles = from e in arrayList
                        //    orderby e.InitDateTime
                        //    select e;
                        totalcount = arrayList.Count();

                        return Ok(new{total= totalcount,result});
                    }
                    else
                    {
                        foreach (var content in data.ToList())
                        {
                            NewArticle newartary = new NewArticle();
                            newartary.ArticleID = content.ID;
                            newartary.UserName = content.UserName;
                            newartary.Title = content.Title;
                            newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                            newartary.ArtInfo = content.Introduction;
                            newartary.Articlecategory = content.Articlecategory.Name;
                            newartary.Isfree = content.IsFree;
                            newartary.Lovecount = content.Lovecount;
                            newartary.InitDateTime = content.InitDate;

                            arrayList.Add(newartary);
                        }

                        foreach (var content in dataNormal.ToList())
                        {

                            NewArticle newartary = new NewArticle();
                            newartary.ArticleID = content.ID;
                            newartary.UserName = content.UserName;
                            newartary.Title = content.Title;
                            newartary.ArtPic = "null";
                            newartary.ArtInfo = "null";
                            newartary.Articlecategory = content.Articlecategory.Name;
                            newartary.Isfree = content.IsFree;
                            newartary.Lovecount = content.Lovecount;
                            newartary.InitDateTime = content.InitDate;

                            arrayList.Add(newartary);


                        }

                        int page = (nowpage - 1) * showcount;
                        //排序依照日期
                        var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);


                        return Ok(result);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// 最新文章排列(前三筆
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle()
        {
            var data = from q in db.Articles 
                    where q.IsPush == true
                       select q;
            var dataNormal = from q in db.ArticleNormals
                where q.IsPush == true
                select q;
            if (data != null && dataNormal != null)
            {
                try
                {
                    List<NewArticle> arrayList = new List<NewArticle>();

                    foreach (var content in data.ToList())
                    {
                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                        newartary.ArtInfo = content.Introduction;
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);
                    }

                    foreach (var content in dataNormal.ToList())
                    {

                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = "null";
                        newartary.ArtInfo = "null";
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);


                    }
                    //排序依照日期 desending遞減
                    //用Take表示拿取幾筆資料
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(3);
                    //另一種寫法
                    //var result = from e in arrayList
                    //    orderby e.InitDateTime descending 
                    //    select e;
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// 最熱門文章排列(前四筆
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("lovearticle")]
        public IHttpActionResult Lovearticle()
        {
            var data = from q in db.Articles
                    where q.IsPush == true & q.Lovecount >0
                       select q;
            var dataNormal = from q in db.ArticleNormals
                where q.IsPush == true & q.Lovecount >0
                select q;
            if (data != null && dataNormal != null)
            {
                try
                {
                    List<NewArticle> arrayList = new List<NewArticle>();

                    foreach (var content in data.ToList())
                    {
                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                        newartary.ArtInfo = content.Introduction;
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);
                    }

                    foreach (var content in dataNormal.ToList())
                    {

                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = "null";
                        newartary.ArtInfo = "null";
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);


                    }
                    //排序依照日期 desending遞減
                    var result = arrayList.OrderByDescending(x => x.Lovecount).Take(4);
                    
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 關鍵字搜尋文章標題
        /// </summary>
        /// <param name="keywords">關鍵字</param>
        /// <param name="nowpage">現在的頁數(一開始就是第一頁,請填1)</param>
        /// <param name="datacount">總共幾筆資料(一開始請填0,後端會傳回總共幾筆)</param>
        /// <param name="showcount">顯示筆數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Seekstringarticle(string keywords,int nowpage,int datacount,int showcount)
        {
            var dataarticle = from d in db.Articles
                where (d.Title.Contains(keywords))
                select d;
            
            var dataarticleN = from d in db.ArticleNormals
                where (d.Title.Contains(keywords))
                select d;
            if (dataarticle == null && dataarticleN == null)
            {
                return NotFound();
            }
            try
            {
                var articlelist = dataarticle.ToList();
                var articleNlist = dataarticleN.ToList();
                List<NewArticle> arrayList = new List<NewArticle>();
                if (datacount == 0)
                {
                    

                    foreach (var content in articlelist)
                    {
                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                        newartary.ArtInfo = content.Introduction;
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);
                    }

                    foreach (var content in articleNlist)
                    {

                        NewArticle newartary = new NewArticle();
                        newartary.ArticleID = content.ID;
                        newartary.UserName = content.UserName;
                        newartary.Title = content.Title;
                        newartary.ArtPic = "null";
                        newartary.ArtInfo = "null";
                        newartary.Articlecategory = content.Articlecategory.Name;
                        newartary.Isfree = content.IsFree;
                        newartary.Lovecount = content.Lovecount;
                        newartary.InitDateTime = content.InitDate;

                        arrayList.Add(newartary);


                    }
                    //排序依照日期 desending遞減
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                    //另一種寫法
                    //var result = from e in arrayList
                    //             orderby e.InitDateTime descending
                    //             select e;
                    int resultcount = arrayList.Count();
                    
                    return Ok(new{DataCount = resultcount,result});
                }
                

                foreach (var content in articlelist)
                {
                    NewArticle newartary = new NewArticle();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Title = content.Title;
                    newartary.ArtPic = content.FirstPicName + "." + content.FirstPicFileName;
                    newartary.ArtInfo = content.Introduction;
                    newartary.Articlecategory = content.Articlecategory.Name;
                    newartary.Isfree = content.IsFree;
                    newartary.Lovecount = content.Lovecount;
                    newartary.InitDateTime = content.InitDate;

                    arrayList.Add(newartary);
                }

                foreach (var content in articleNlist)
                {

                    NewArticle newartary = new NewArticle();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Title = content.Title;
                    newartary.ArtPic = "null";
                    newartary.ArtInfo = "null";
                    newartary.Articlecategory = content.Articlecategory.Name;
                    newartary.Isfree = content.IsFree;
                    newartary.Lovecount = content.Lovecount;
                    newartary.InitDateTime = content.InitDate;

                    arrayList.Add(newartary);


                }

                int takepage = (nowpage - 1) * showcount;
                var resultdata= arrayList.OrderByDescending(x => x.InitDateTime).Skip(takepage).Take(showcount);
                return Ok(resultdata);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
