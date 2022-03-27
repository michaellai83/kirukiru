using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using testdatamodel.listclass;
using testdatamodel.Models;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 首頁顯示一般文章
    /// </summary>
    public class HomepagenormalController : ApiController
    {
        ProjectDb db = new ProjectDb();
        /// <summary>
        /// 依類別搜尋一般文章(按照時間排列)
        /// </summary>
        /// <param name="artlog">類別ID</param>
        /// <param name="nowpage">現在的頁數(一開始請填1)</param>
        /// <param name="showcount">要顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekArticle(int artlog, int nowpage, int showcount)
        {

            var datanormal = from a in db.ArticleNormals
                             where (a.ArticlecategoryId == artlog &
                                    a.IsPush == true)
                             select a;
            int pagecount = datanormal.Count();
            if ( datanormal != null)
            {

                List<NormalArticles> arrayList = new List<NormalArticles>();

                foreach (var content in datanormal.ToList())
                {

                    NormalArticles newartary = new NormalArticles();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Introduction = content.Introduction;
                    newartary.Title = content.Title;
                    newartary.Articlecategory = content.Articlecategory.Name;
                    newartary.Isfree = content.IsFree;
                    newartary.Lovecount = content.Lovecount;
                    newartary.InitDateTime = content.InitDate;

                    arrayList.Add(newartary);

                }
                if (nowpage == 1)
                {
                    //排序依照日期
                    //var result = from e in arrayList
                    //    orderby e.InitDateTime
                    //    select e;
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                    pagecount = arrayList.Count();
                    return Ok(new
                    {
                        success = true,
                        total = pagecount,
                        data = result
                    });
                   
                }
                else
                {
                   

                    int page = (nowpage - 1) * showcount;
                    //排序依照日期

                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
                    return Ok(new
                    {
                        success = true,
                        total = pagecount,
                        data =result
                    });
                }

            }
            else
            {
                return Ok(new
                {
                    success=false,
                    message="類別錯誤"
                });
            }

        }
        /// <summary>
        /// 依照時間搜尋一般文章
        /// </summary>
        /// <param name="datetime1">較早的時間(ex:2022.03.08)</param>
        /// <param name="datetime2">較晚的時間(ex:2022.03.10)</param>
        /// <param name="nowpage">現在頁數(一開始請直接傳1)</param>
        /// <param name="showcount">顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekTimeArticle(string datetime1, string datetime2, int nowpage, int showcount)
        {
            var datetime01 = DateTime.Parse(datetime1);
            var datetime02 = DateTime.Parse(datetime2).AddDays(1);

            var dataNormal = from q in db.ArticleNormals
                             where q.InitDate >= datetime01 && q.InitDate < datetime02 & q.IsPush == true
                             select q;
            int totalcount = dataNormal.Count();
            if (dataNormal != null)
            {
                List<NormalArticles> arrayList = new List<NormalArticles>();
                foreach (var content in dataNormal.ToList())
                {

                    NormalArticles newartary = new NormalArticles();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Introduction = content.Introduction;
                    newartary.Title = content.Title;
                    newartary.Articlecategory = content.Articlecategory.Name;
                    newartary.Isfree = content.IsFree;
                    newartary.Lovecount = content.Lovecount;
                    newartary.InitDateTime = content.InitDate;

                    arrayList.Add(newartary);


                }

                if (nowpage == 1)
                {

                    
                    //排序依照日期
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                    //var resultArticles = from e in arrayList
                    //    orderby e.InitDateTime
                    //    select e;
                    totalcount = arrayList.Count();

                    return Ok(new
                    {
                        success=true,
                        total = totalcount,
                        data=result
                    });
                }
                else
                {

                    int page = (nowpage - 1) * showcount;
                    //排序依照日期
                    var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);


                    return Ok(new
                    {
                        success=true,
                        total = totalcount,
                        data =result
                    });
                }
            }
            else
            {
                return Ok(new
                {
                    success=false,
                    message="時間或者格式錯誤"
                });
            }
        }
        /// <summary>
        /// 最新的一般文章(前三筆)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle()
        {

            var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true).ToList();
            //var dataNormal = from q in db.ArticleNormals
            //                 where q.IsPush == true
            //                 select q;
            if (dataNormal != null)
            {
                List<NormalArticles> arrayList = new List<NormalArticles>();


                foreach (var content in dataNormal)
                {

                    NormalArticles newartary = new NormalArticles();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Title = content.Title;
                    newartary.Introduction = content.Introduction;
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
                return Ok(new
                {
                    success=true,
                    data=result
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "應該不會有錯誤拉"
                });
            }
        }
        /// <summary>
        /// 最熱門的一般文章(前四筆)
        /// </summary>
        /// <returns></returns>
        [Route("api/Homepagenormal/lovenormalarticle")]
        [HttpGet]
        public IHttpActionResult Lovearticle()
        {
            var dataNormal = from q in db.ArticleNormals
                             where q.IsPush == true & q.Lovecount > 0
                             select q;
            if ( dataNormal != null)
            {
                List<NormalArticles> arrayList = new List<NormalArticles>();


                foreach (var content in dataNormal.ToList())
                {

                    NormalArticles newartary = new NormalArticles();
                    newartary.ArticleID = content.ID;
                    newartary.UserName = content.UserName;
                    newartary.Title = content.Title;
                    newartary.Introduction = content.Introduction;
                    newartary.Articlecategory = content.Articlecategory.Name;
                    newartary.Isfree = content.IsFree;
                    newartary.Lovecount = content.Lovecount;
                    newartary.InitDateTime = content.InitDate;

                    arrayList.Add(newartary);


                }
                //排序依照日期 desending遞減
                var result = arrayList.OrderByDescending(x => x.Lovecount).Take(4);

                return Ok(new
                {
                    success=true,
                    data=result
                });
            }
            else
            {
                return Ok(new
                {
                    success=false,
                    message="我也不知道啥錯誤"
                });
            }
        }
        /// <summary>
        /// 依關鍵字搜尋一般文章
        /// </summary>
        /// <param name="keywords">關鍵字</param>
        /// <param name="nowpage">現在的頁數(一開始就是第一頁,請填1)</param>
        /// <param name="showcount">顯示筆數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Seekstringarticle(string keywords, int nowpage, int showcount)
        {
            //多重查詢
            //var a = db.Articles.AsQueryable();
            //if (!string.IsNullOrWhiteSpace(keywords))
            //{
            //    a = a.Where(x => x.Title.Contains(keywords));

            //}
            //a = a.Where(x => x.InitDate >= DateTime.Today);
            //var b = a.ToList();
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return Ok(new
                {
                    sucess = false,
                    message = "請輸入關鍵字"
                });
            }
            var a = db.ArticleNormals.AsQueryable();
            a = a.Where(x => x.Title.Contains(keywords));
            var articleNlist = a.ToList();
            int resultcount = articleNlist.Count();

            //var dataarticleN = from d in db.ArticleNormals
            //                   where (d.Title.Contains(keywords))
            //                   select d;

            //var articleNlist = dataarticleN.ToList();
            List<NormalArticles> arrayList = new List<NormalArticles>();
            foreach (var content in articleNlist)
            {

                NormalArticles newartary = new NormalArticles();
                newartary.ArticleID = content.ID;
                newartary.UserName = content.UserName;
                newartary.Title = content.Title;
                newartary.Introduction = content.Introduction;
                newartary.Articlecategory = content.Articlecategory.Name;
                newartary.Isfree = content.IsFree;
                newartary.Lovecount = content.Lovecount;
                newartary.InitDateTime = content.InitDate;
                arrayList.Add(newartary);


            }
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //             orderby e.InitDateTime descending
                //             select e;

                return Ok(new
                {
                    success=true,
                    total = resultcount,
                    data=result
                });
            }
            else
            {
                int takepage = (nowpage - 1) * showcount;
                var resultdata = arrayList.OrderByDescending(x => x.InitDateTime).Skip(takepage).Take(showcount);
                return Ok(new
                {
                    success=true,
                    total = resultcount,
                    data =resultdata
                });
            }




        }
    }
}
