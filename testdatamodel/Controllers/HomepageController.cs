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
        [JwtAuthFilter]
        public IHttpActionResult SeekArticle(int articlecategoryId, int nowpage,int showcount)
        {
            //有鎖JWT 如果到時候確認訂閱方法可以用再把全部都加上去JWT
            var member = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var memberorder = db.Members.FirstOrDefault(x => x.ID == member).Orderlists.ToList();
            List<string> checkorder = new List<string>();
            if (memberorder != null)
            {
                //拿取訂閱資料(要成功交易完才能有資料
                foreach (var str in memberorder)
                {
                    if (str.Issuccess == true)
                    {
                        checkorder.Add(str.AuthorName);
                    }
                    
                }
                
                
            }
            int ordernum = checkorder.Count;
            var data = from a in db.Articles
                where (a.ArticlecategoryId == articlecategoryId &
                       a.IsPush == true)
                select a;
            int pagecount = data.Count();
            //var datanormal = from a in db.ArticleNormals
            //    where (a.ArticlecategoryId == artlog &
            //           a.IsPush == true)
            //    select a;
            if (data != null /*&& datanormal != null*/)
            {
                
                List<NewArticle> arrayList = new List<NewArticle>();
                foreach (var content in data.ToList())
                {

                    //var good = db.Goods.Where(m => m.ArticleId == artid).Count();//不要在迴圈裡面開資料庫
                    NewArticle newartary = new NewArticle();
                    newartary.artId = content.ID;
                    newartary.username = content.UserName;
                    newartary.author = content.AuthorName;
                    newartary.authorPic = content.AuthorPic;
                    newartary.title = content.Title;
                    newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                    newartary.introduction = content.Introduction;
                    newartary.articlecategoryId = content.ArticlecategoryId;
                    newartary.artArtlog = content.Articlecategory.Name;
                    newartary.isFree = content.IsFree;
                    newartary.lovecount = content.Lovecount;
                    newartary.ArtInitDate = content.InitDate;

                    arrayList.Add(newartary);
                }
                //這邊是去確認有訂閱的作者會去把isfree狀態都改成免費
                if (ordernum != 0)
                {
                    foreach (var str in arrayList)
                    {

                        for (int i = 0; i < ordernum; i++)
                        {
                            if (checkorder[i] == str.username && str.isFree == false)
                            {
                                str.isFree = true;
                                break;
                            }
                        }
                    }
                }
                if (nowpage == 1)
                {
                    
                    
                   
                    //foreach (var content in datanormal.ToList())
                    //{

                    //    NewArticle newartary = new NewArticle();
                    //    newartary.ArticleID = content.ID;
                    //    newartary.UserName = content.UserName;
                    //    newartary.Title = content.Title;
                    //    newartary.ArtPic = "null";
                    //    newartary.ArtInfo = "null";
                    //    newartary.Articlecategory = content.Articlecategory.Name;
                    //    newartary.Isfree = content.IsFree;
                    //    newartary.Lovecount = content.Lovecount;
                    //    newartary.InitDateTime = content.InitDate;

                    //    arrayList.Add(newartary);


                    //}
                    //排序依照日期
                    //var result = from e in arrayList
                    //    orderby e.InitDateTime
                    //    select e;
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
                    pagecount = arrayList.Count();
                    return Ok(new
                    {
                        success=true,
                        total=pagecount,
                        data=result
                    });
                }
                else
                {

                    //foreach (var content in datanormal.ToList())
                    //{

                    //    NewArticle newartary = new NewArticle();
                    //    newartary.ArticleID = content.ID;
                    //    newartary.UserName = content.UserName;
                    //    newartary.Title = content.Title;
                    //    newartary.ArtPic = "null";
                    //    newartary.ArtInfo = "null";
                    //    newartary.Articlecategory = content.Articlecategory.Name;
                    //    newartary.Isfree = content.IsFree;
                    //    newartary.Lovecount = content.Lovecount;
                    //    newartary.InitDateTime = content.InitDate;

                    //    arrayList.Add(newartary);


                    //}

                    int page = (nowpage - 1) * showcount;
                    //排序依照日期
                    
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
                    return Ok(new
                    {
                        success=true,
                        total = pagecount,
                        data =result
                    });
                }
                
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "類別錯誤"
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
            var data = from q in db.Articles
                       where (q.InitDate >= datetime01 && q.InitDate < datetime02 & q.IsPush == true)
                       select q;
            int totalcount = data.Count();
            //var dataNormal = from q in db.ArticleNormals
            //    where q.InitDate >= datetime01 && q.InitDate <datetime02 & q.IsPush == true
            //    select q;
            if (data != null /*&& dataNormal != null*/)
            {
                List<NewArticle> arrayList = new List<NewArticle>();
                foreach (var content in data.ToList())
                {
                    NewArticle newartary = new NewArticle();
                    newartary.artId = content.ID;
                    newartary.username = content.UserName;
                    newartary.author = content.AuthorName;
                    newartary.authorPic = content.AuthorPic;
                    newartary.title = content.Title;
                    newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                    newartary.introduction = content.Introduction;
                    newartary.artArtlog = content.Articlecategory.Name;
                    newartary.articlecategoryId = content.ArticlecategoryId;
                    newartary.isFree = content.IsFree;
                    newartary.lovecount = content.Lovecount;
                    newartary.ArtInitDate = content.InitDate;

                    arrayList.Add(newartary);
                }

                if (nowpage == 1)
                {
                    

                    //foreach (var content in dataNormal.ToList())
                    //{

                    //    NewArticle newartary = new NewArticle();
                    //    newartary.ArticleID = content.ID;
                    //    newartary.UserName = content.UserName;
                    //    newartary.Title = content.Title;
                    //    newartary.ArtPic = "null";
                    //    newartary.ArtInfo = "null";
                    //    newartary.Articlecategory = content.Articlecategory.Name;
                    //    newartary.Isfree = content.IsFree;
                    //    newartary.Lovecount = content.Lovecount;
                    //    newartary.InitDateTime = content.InitDate;

                    //    arrayList.Add(newartary);


                    //}
                    //排序依照日期
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
                    //var resultArticles = from e in arrayList
                    //    orderby e.InitDateTime
                    //    select e;
                    totalcount = arrayList.Count();

                    return Ok(new
                    {
                        success = true,
                        total = totalcount,
                        data = result
                    });
                }
                else
                {

                    //foreach (var content in dataNormal.ToList())
                    //{

                    //    NewArticle newartary = new NewArticle();
                    //    newartary.ArticleID = content.ID;
                    //    newartary.UserName = content.UserName;
                    //    newartary.Title = content.Title;
                    //    newartary.ArtPic = "null";
                    //    newartary.ArtInfo = "null";
                    //    newartary.Articlecategory = content.Articlecategory.Name;
                    //    newartary.Isfree = content.IsFree;
                    //    newartary.Lovecount = content.Lovecount;
                    //    newartary.InitDateTime = content.InitDate;

                    //    arrayList.Add(newartary);


                    //}

                    int page = (nowpage - 1) * showcount;
                    //排序依照日期
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);


                    return Ok(new
                    {
                        success = true,
                        total = totalcount,
                        data = result
                    });
                }
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "時間或者格式錯誤"
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
            var data = from q in db.Articles 
                    where q.IsPush == true
                       select q;
            //var dataNormal = from q in db.ArticleNormals
            //    where q.IsPush == true
            //    select q;
            if (data != null /*&& dataNormal != null*/)
            {
                List<NewArticle> arrayList = new List<NewArticle>();

                foreach (var content in data.ToList())
                {
                    NewArticle newartary = new NewArticle();
                    newartary.artId = content.ID;
                    newartary.username = content.UserName;
                    newartary.author = content.AuthorName;
                    newartary.authorPic = content.AuthorPic;
                    newartary.title = content.Title;
                    newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                    newartary.introduction = content.Introduction;
                    newartary.artArtlog = content.Articlecategory.Name;
                    newartary.articlecategoryId = content.ArticlecategoryId;
                    newartary.isFree = content.IsFree;
                    newartary.lovecount = content.Lovecount;
                    newartary.ArtInitDate = content.InitDate;

                    arrayList.Add(newartary);
                }

                //foreach (var content in dataNormal.ToList())
                //{

                //    NewArticle newartary = new NewArticle();
                //    newartary.ArticleID = content.ID;
                //    newartary.UserName = content.UserName;
                //    newartary.Title = content.Title;
                //    newartary.ArtPic = "null";
                //    newartary.ArtInfo = "null";
                //    newartary.Articlecategory = content.Articlecategory.Name;
                //    newartary.Isfree = content.IsFree;
                //    newartary.Lovecount = content.Lovecount;
                //    newartary.InitDateTime = content.InitDate;

                //    arrayList.Add(newartary);


                //}
                var total = arrayList.Count;
                if (nowpage == 1)
                {
                    //排序依照日期 desending遞減
                    //用Take表示拿取幾筆資料
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
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
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
                    return Ok(new
                    {
                        success = true,
                        total = total,
                        data = result
                    });
                }
               
            }
            else
            {
                return Ok(new
                {
                    success=false,
                    message="應該不會有錯誤吧"
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
            var data = from q in db.Articles
                    where q.IsPush == true & q.Lovecount >0
                       select q;
            //var dataNormal = from q in db.ArticleNormals
            //    where q.IsPush == true & q.Lovecount >0
            //    select q;
            if (data != null /*&& dataNormal != null*/)
            {
                List<NewArticle> arrayList = new List<NewArticle>();

                foreach (var content in data.ToList())
                {
                    NewArticle newartary = new NewArticle();
                    newartary.artId = content.ID;
                    newartary.username = content.UserName;
                    newartary.author = content.AuthorName;
                    newartary.authorPic = content.AuthorPic;
                    newartary.title = content.Title;
                    newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                    newartary.introduction = content.Introduction;
                    newartary.artArtlog = content.Articlecategory.Name;
                    newartary.articlecategoryId = content.ArticlecategoryId;
                    newartary.isFree = content.IsFree;
                    newartary.lovecount = content.Lovecount;
                    newartary.ArtInitDate = content.InitDate;

                    arrayList.Add(newartary);
                }

                //foreach (var content in dataNormal.ToList())
                //{

                //    NewArticle newartary = new NewArticle();
                //    newartary.ArticleID = content.ID;
                //    newartary.UserName = content.UserName;
                //    newartary.Title = content.Title;
                //    newartary.ArtPic = "null";
                //    newartary.ArtInfo = "null";
                //    newartary.Articlecategory = content.Articlecategory.Name;
                //    newartary.Isfree = content.IsFree;
                //    newartary.Lovecount = content.Lovecount;
                //    newartary.InitDateTime = content.InitDate;

                //    arrayList.Add(newartary);


                //}
                int total = arrayList.Count;
                if (nowpage == 1)
                {
                    //排序依照日期 desending遞減
                    var result = arrayList.OrderByDescending(x => x.lovecount).Take(4);

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
                    var result = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
                    return Ok(new
                    {
                        success = true,
                        total = total,
                        data = result
                    });
                }
               
            }
            else
            {
                return Ok(new
                {
                    success=false,
                    message="我也不知道啥錯誤應該是沒資料拉"
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
            var dataarticle = from d in db.Articles
                where (d.Title.Contains(keywords))
                select d;
            int datacount = dataarticle.Count();
            //var dataarticleN = from d in db.ArticleNormals
            //    where (d.Title.Contains(keywords))
            //    select d;

            var articlelist = dataarticle.ToList();
            //var articleNlist = dataarticleN.ToList();
            List<NewArticle> arrayList = new List<NewArticle>();
            foreach (var content in articlelist)
            {
                NewArticle newartary = new NewArticle();
                newartary.artId = content.ID;
                newartary.username = content.UserName;
                newartary.author = content.AuthorName;
                newartary.authorPic = content.AuthorPic;
                newartary.title = content.Title;
                newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                newartary.introduction = content.Introduction;
                newartary.artArtlog = content.Articlecategory.Name;
                newartary.articlecategoryId = content.ArticlecategoryId;
                newartary.isFree = content.IsFree;
                newartary.lovecount = content.Lovecount;
                newartary.ArtInitDate = content.InitDate;

                arrayList.Add(newartary);
            }
            if (nowpage == 1)
            {

                //foreach (var content in articleNlist)
                //{

                //    NewArticle newartary = new NewArticle();
                //    newartary.ArticleID = content.ID;
                //    newartary.UserName = content.UserName;
                //    newartary.Title = content.Title;
                //    newartary.ArtPic = "null";
                //    newartary.ArtInfo = "null";
                //    newartary.Articlecategory = content.Articlecategory.Name;
                //    newartary.Isfree = content.IsFree;
                //    newartary.Lovecount = content.Lovecount;
                //    newartary.InitDateTime = content.InitDate;

                //    arrayList.Add(newartary);


                //}
                //排序依照日期 desending遞減
                var result = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
                //另一種寫法
                //var result = from e in arrayList
                //             orderby e.InitDateTime descending
                //             select e;

                return Ok(new
                {
                    success=true,
                    total = datacount, 
                    data=result
                });
            }
            else
            {

                //foreach (var content in articleNlist)
                //{

                //    NewArticle newartary = new NewArticle();
                //    newartary.ArticleID = content.ID;
                //    newartary.UserName = content.UserName;
                //    newartary.Title = content.Title;
                //    newartary.ArtPic = "null";
                //    newartary.ArtInfo = "null";
                //    newartary.Articlecategory = content.Articlecategory.Name;
                //    newartary.Isfree = content.IsFree;
                //    newartary.Lovecount = content.Lovecount;
                //    newartary.InitDateTime = content.InitDate;

                //    arrayList.Add(newartary);


                //}

                int takepage = (nowpage - 1) * showcount;
                var resultdata = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(takepage).Take(showcount);
                return Ok(new
                    {
                        success=true,
                        total = datacount,
                        data = resultdata
                    });
            }


           

        }
    }
}
