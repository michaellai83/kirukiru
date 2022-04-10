using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using testdatamodel.JWT;
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
        /// 依類別取切切文章(按照時間最新排列(有完成訂閱作者的話他的文章都會解鎖
        /// </summary>
        /// <param name="articlecategoryId">類別的數字值</param>
        /// <param name="nowpage">現在的頁數(一開始請填1)</param>
        /// <param name="showcount">要顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekArticle(int articlecategoryId, int nowpage,int showcount)
        {
            var artLog = db.Articlecategory.FirstOrDefault(x => x.Id == articlecategoryId);
            if (artLog == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "類別錯誤"
                });
            }
            //var data = db.Articles.Where(m => m.ArticlecategoryId == articlecategoryId).Where(m => m.IsPush == true).Select(x=>new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).ToList();

            var data = db.Articles.Where(m => m.ArticlecategoryId == articlecategoryId && m.IsPush == true).Join(db.Members,
                a=>a.UserName,
                b=>b.UserName,
                (a,b)=>new
            {
                artId = a.ID,
                author = b.Name,
                authorPic = b.PicName + "." + b.FileName,
                username = b.UserName,
                title = a.Title,
                firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                introduction = a.Introduction,
                artArtlog = a.Articlecategory.Name,
                articlecategoryId = a.ArticlecategoryId,
                isFree = a.IsFree,
                lovecount = a.Lovecount,
                messageCount = a.Messages.Count,
                artInitDate = a.InitDate
            }).Select(x => new
            {
                artId = x.artId,
                author = x.author,
                authorPic = x.authorPic,
                username = x.username,
                title = x.title,
                firstPhoto = x.firstPhoto,
                introduction = x.introduction,
                artArtlog = x.artArtlog,
                articlecategoryId = x.articlecategoryId,
                isFree = x.isFree,
                lovecount = x.lovecount,
                messageCount = x.messageCount,
                artInitDate = x.artInitDate
            }).ToList();

            int pagecount = data.Count();

            if (nowpage == 1)
            {



                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                pagecount = data.Count();
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

                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
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
        /// 依照時間範圍搜尋切切文章
        /// </summary>
        /// <param name="datetime1">較早的時間(ex:2022.03.08)</param>
        /// <param name="datetime2">較晚的時間(ex:2022.03.10)</param>
        /// <param name="nowpage">現在頁數(一開始請直接傳1)</param>
        /// <param name="showcount">顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekTimeArticle(string datetime1, string datetime2,int nowpage,int showcount )
        {
            var datetime01 = DateTime.Parse(datetime1);
            var datetime02 = DateTime.Parse(datetime2).AddDays(1);
            //var data = db.Articles.Where(m => m.InitDate >= datetime01).Where(m => m.InitDate < datetime02)
            //    .Where(m => m.IsPush == true).Select(x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        artInitDate = x.InitDate
            //    }).ToList();

            var data = db.Articles.Where(m => m.InitDate >= datetime01 && m.InitDate < datetime02 &&m.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
            {
                artId = a.ID,
                author = b.Name,
                authorPic = b.PicName + "." + b.FileName,
                username = b.UserName,
                title = a.Title,
                firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                introduction = a.Introduction,
                artArtlog = a.Articlecategory.Name,
                articlecategoryId = a.ArticlecategoryId,
                isFree = a.IsFree,
                lovecount = a.Lovecount,
                messageCount = a.Messages.Count,
                artInitDate = a.InitDate
                }).Select(x => new
                {
                    artId = x.artId,
                    author = x.author,
                    authorPic = x.authorPic,
                    username = x.username,
                    title = x.title,
                    firstPhoto = x.firstPhoto,
                    introduction = x.introduction,
                    artArtlog = x.artArtlog,
                    articlecategoryId = x.articlecategoryId,
                    isFree = x.isFree,
                    lovecount = x.lovecount,
                    messageCount = x.messageCount,
                    artInitDate = x.artInitDate
                }).ToList();

            int totalcount = data.Count();
            //var dataNormal = from q in db.ArticleNormals
            //    where q.InitDate >= datetime01 && q.InitDate <datetime02 & q.IsPush == true
            //    select q;
            if (nowpage == 1)
            {

                //排序依照日期
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                //var resultArticles = from e in arrayList
                //    orderby e.InitDateTime
                //    select e;
                totalcount = data.Count();

                return Ok(new
                {
                    success = true,
                    total = totalcount,
                    data = result
                });
            }
            else
            {

                int page = (nowpage - 1) * showcount;
                //排序依照日期
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = totalcount,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最新的切切文章排列
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle(int nowpage, int showcount)
        {
            //var data = db.Articles.Where(m => m.IsPush == true).Select(x => new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).ToList();
            var data = db.Articles.Where(m => m.IsPush == true).
                Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    artArtlog = a.Articlecategory.Name,
                    articlecategoryId = a.ArticlecategoryId,
                    isFree = a.IsFree,
                    lovecount = a.Lovecount,
                    messageCount = a.Messages.Count,
                    artInitDate = a.InitDate
                }).Select(x => new
                {
                    artId = x.artId,
                    author = x.author,
                    authorPic = x.authorPic,
                    username = x.username,
                    title = x.title,
                    firstPhoto = x.firstPhoto,
                    introduction = x.introduction,
                    artArtlog = x.artArtlog,
                    articlecategoryId = x.articlecategoryId,
                    isFree = x.isFree,
                    lovecount = x.lovecount,
                    messageCount = x.messageCount,
                    artInitDate = x.artInitDate
                }).ToList();

            var total = data.Count;
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                //用Take表示拿取幾筆資料
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //    orderby e.InitDateTime descending 
                //    select e;
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最熱門的切切文章排列
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [Route("api/Homepage/lovearticle")]
        [HttpGet]
        
        public IHttpActionResult Lovearticle(int nowpage, int showcount)
        {
            //var data = db.Articles.Where(m => m.IsPush == true).Where(m => m.Lovecount > 0).Select(x => new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).ToList();
            var data = db.Articles.Where(m => m.IsPush == true && m.Lovecount > 0).
                Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    artArtlog = a.Articlecategory.Name,
                    articlecategoryId = a.ArticlecategoryId,
                    isFree = a.IsFree,
                    lovecount = a.Lovecount,
                    messageCount = a.Messages.Count,
                    artInitDate = a.InitDate
                }).Select(x => new
            {
                artId = x.artId,
                author = x.author,
                authorPic = x.authorPic,
                username = x.username,
                title = x.title,
                firstPhoto = x.firstPhoto,
                introduction = x.introduction,
                artArtlog = x.artArtlog,
                articlecategoryId = x.articlecategoryId,
                isFree = x.isFree,
                lovecount = x.lovecount,
                messageCount = x.messageCount,
                artInitDate = x.artInitDate
                }).ToList();

            int total = data.Count;
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = data.OrderByDescending(x => x.lovecount).Take(4);

                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }

        /// <summary>
        /// 關鍵字搜尋切切文章標題
        /// </summary>
        /// <param name="keywords">關鍵字</param>
        /// <param name="nowpage">現在的頁數(一開始就是第一頁,請填1)</param>
        /// <param name="showcount">顯示筆數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Seekstringarticle(string keywords,int nowpage,int showcount)
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
                    sucess=false,
                    message="請輸入關鍵字"
                });
            }

            //var dataarticle = db.Articles.Where(m => m.Title.Contains(keywords)).Where(m => m.IsPush == true).Select(
            //    x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        artInitDate = x.InitDate
            //    }).ToList();

            var dataarticle = db.Articles.Where(m => m.Title.Contains(keywords) && m.IsPush == true).
                Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    artArtlog = a.Articlecategory.Name,
                    articlecategoryId = a.ArticlecategoryId,
                    isFree = a.IsFree,
                    lovecount = a.Lovecount,
                    messageCount = a.Messages.Count,
                    artInitDate = a.InitDate
                }).Select(
                x => new
                {
                    artId = x.artId,
                    author = x.author,
                    authorPic = x.authorPic,
                    username = x.username,
                    title = x.title,
                    firstPhoto = x.firstPhoto,
                    introduction = x.introduction,
                    artArtlog = x.artArtlog,
                    articlecategoryId = x.articlecategoryId,
                    isFree = x.isFree,
                    lovecount = x.lovecount,
                    messageCount = x.messageCount,
                    artInitDate = x.artInitDate
                }).ToList();
            //var dataarticle = from d in db.Articles
            //    where (d.Title.Contains(keywords)&& d.IsPush == true)
            //    select d;
            //var articlelist = dataarticle.ToList();
           
           
            if (nowpage == 1)
            {

                //排序依照日期 desending遞減
                var result = dataarticle.OrderByDescending(x => x.artInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //             orderby e.InitDateTime descending
                //             select e;

                return Ok(new
                {
                    success=true,
                    total = dataarticle.Count, 
                    data=result
                });
            }
            else
            {


                int takepage = (nowpage - 1) * showcount;
                var resultdata = dataarticle.OrderByDescending(x => x.artInitDate).Skip(takepage).Take(showcount);
                return Ok(new
                    {
                        success=true,
                        total = dataarticle.Count,
                        data = resultdata
                    });
            }


           

        }
    }
}
