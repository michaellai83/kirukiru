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
        /// <param name="articlecategoryId">類別ID</param>
        /// <param name="nowpage">現在的頁數(一開始請填1)</param>
        /// <param name="showcount">要顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult SeekArticle(int articlecategoryId, int nowpage, int showcount)
        {
            //var datanormal = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId)
            //    .Where(m => m.IsPush == true).Select(x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        messageCount = x.MessageNormals.Count,
            //        artInitDate = x.InitDate
            //    }).ToList();
            //var datanormal = from a in db.ArticleNormals
            //                 where (a.ArticlecategoryId == articlecategoryId &
            //                        a.IsPush == true)
            //                 select a;
            var artLog = db.Articlecategory.FirstOrDefault(x => x.Id == articlecategoryId);
            if (artLog == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "類別錯誤"
                });
            }
            
            var datanormal = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId && m.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                })
                .Select(x => new
                {
                    artId = x.artId,
                    username = x.userName,
                    author = x.authorName,
                    authorPic = x.authorPic,
                    introduction = x.introduction,
                    title = x.title,
                    articlecategoryId = x.articlecategoryId,
                    artArtlog = x.artArtlog,
                    main = x.main,
                    isFree = x.isFree,
                    messageCount = x.messageCount,
                    lovecount = x.lovecount,
                    artInitDate = x.artInitDate
                }).ToList();
            int pagecount = datanormal.Count();
            if (nowpage == 1)
            {
                //排序依照日期
                //var result = from e in arrayList
                //    orderby e.InitDateTime
                //    select e;
                var result = datanormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = datanormal.Count,
                    data = result
                });

            }
            else
            {


                int page = (nowpage - 1) * showcount;
                //排序依照日期

                var result = datanormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = datanormal.Count,
                    data = result
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
           
            //var dataNormal = db.ArticleNormals.Where(m => m.InitDate >= datetime01).Where(m => m.InitDate < datetime02)
            //    .Where(m => m.IsPush).Select(x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        messageCount = x.MessageNormals.Count,
            //        artInitDate = x.InitDate
            //    });
            //var dataNormal = from q in db.ArticleNormals
            //                 where q.InitDate >= datetime01 && q.InitDate < datetime02 & q.IsPush == true
            //                 select q;

            var dataNormal = db.ArticleNormals.Where(m => m.InitDate >= datetime01 && m.InitDate < datetime02 && m.IsPush)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                }).Select(x => new
                {
                    artId = x.artId,
                    username = x.userName,
                    author = x.authorName,
                    authorPic = x.authorPic,
                    introduction = x.introduction,
                    title = x.title,
                    articlecategoryId = x.articlecategoryId,
                    artArtlog = x.artArtlog,
                    main = x.main,
                    isFree = x.isFree,
                    messageCount = x.messageCount,
                    lovecount = x.lovecount,
                    artInitDate = x.artInitDate
                });
            if (nowpage == 1)
            {


                //排序依照日期
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                //var resultArticles = from e in arrayList
                //    orderby e.InitDateTime
                //    select e;

                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count(),
                    data = result
                });
            }
            else
            {

                int page = (nowpage - 1) * showcount;
                //排序依照日期
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);


                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count(),
                    data = result
                });
            }
            
        }
        /// <summary>
        /// 最新的一般文章
        /// </summary>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <param name="nowpage">現在頁數</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NewArticle(int nowpage,int showcount)
        {

            //var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true).Select(x=>new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    messageCount = x.MessageNormals.Count,
            //    artInitDate = x.InitDate
            //}).ToList();
            //var dataNormal = from q in db.ArticleNormals
            //                 where q.IsPush == true
            //                 select q;

            var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                }).Select(x => new
            {
                artId = x.artId,
                username = x.userName,
                author = x.authorName,
                authorPic = x.authorPic,
                introduction = x.introduction,
                title = x.title,
                articlecategoryId = x.articlecategoryId,
                artArtlog = x.artArtlog,
                main = x.main,
                isFree = x.isFree,
                messageCount = x.messageCount,
                lovecount = x.lovecount,
                artInitDate = x.artInitDate
                }).ToList();
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                //用Take表示拿取幾筆資料
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //    orderby e.InitDateTime descending 
                //    select e;
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
        }
        /// <summary>
        /// 最熱門的一般文章
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [Route("api/Homepagenormal/lovenormalarticle")]
        [HttpGet]
        public IHttpActionResult Lovearticle(int nowpage, int showcount)
        {
            //var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true).Where(x => x.Lovecount > 0).Select(x => new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    messageCount = x.MessageNormals.Count,
            //    artInitDate = x.InitDate
            //}).ToList();
            //var dataNormal = from q in db.ArticleNormals
            //                 where q.IsPush == true & q.Lovecount > 0
            //                 select q;

            var dataNormal = db.ArticleNormals.Where(x => x.IsPush == true && x.Lovecount > 0)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                }).Select(x => new
            {
                artId = x.artId,
                username = x.userName,
                author = x.authorName,
                authorPic = x.authorPic,
                introduction = x.introduction,
                title = x.title,
                articlecategoryId = x.articlecategoryId,
                artArtlog = x.artArtlog,
                main = x.main,
                isFree = x.isFree,
                messageCount = x.messageCount,
                lovecount = x.lovecount,
                artInitDate = x.artInitDate
                }).ToList();
            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = dataNormal.OrderByDescending(x => x.lovecount).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = dataNormal.Count,
                    data = result
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
            //var a = db.ArticleNormals.AsQueryable();
            //a = a.Where(x => x.Title.Contains(keywords) && x.IsPush == true);
            //var articleNlist = a.ToList();
            //int resultcount = articleNlist.Count();
            //var dataNormal = db.ArticleNormals.Where(x => x.Title.Contains(keywords)).Where(x => x.IsPush == true)
            //    .Select(x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        messageCount = x.MessageNormals.Count,
            //        artInitDate = x.InitDate
            //    }).ToList();
            var dataNormal = db.ArticleNormals.Where(x => x.Title.Contains(keywords) && x.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                })
                .Select(x => new
                {
                    artId = x.artId,
                    username = x.userName,
                    author = x.authorName,
                    authorPic = x.authorPic,
                    introduction = x.introduction,
                    title = x.title,
                    articlecategoryId = x.articlecategoryId,
                    artArtlog = x.artArtlog,
                    main = x.main,
                    isFree = x.isFree,
                    messageCount = x.messageCount,
                    lovecount = x.lovecount,
                    artInitDate = x.artInitDate
                }).ToList();

            if (nowpage == 1)
            {
                //排序依照日期 desending遞減
                var result = dataNormal.OrderByDescending(x => x.artInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //             orderby e.InitDateTime descending
                //             select e;

                return Ok(new
                {
                    success=true,
                    total = dataNormal.Count,
                    data=result
                });
            }
            else
            {
                int takepage = (nowpage - 1) * showcount;
                var resultdata = dataNormal.OrderByDescending(x => x.artInitDate).Skip(takepage).Take(showcount);
                return Ok(new
                {
                    success=true,
                    total = dataNormal.Count,
                    data =resultdata
                });
            }




        }
    }
}
