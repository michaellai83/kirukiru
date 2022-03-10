using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using testdatamodel.Models;
using testdatamodel.PutData;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 一般文章的API
    /// </summary>
    public class ArticleNormalController : ApiController
    {
        ProjectDb db = new ProjectDb();
        /// <summary>
        /// 添加一般文章
        /// </summary>
        /// <param name="data">回傳給後端的格式</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreatArticleNormal(DataArticleNormal data)
        {
            var username = data.Username;
            var checkusername = db.Members.FirstOrDefault(m => m.UserName == username);
            if (checkusername == null)
            {
                return NotFound();
            }

            try
            {
                
                ArticleNormal article = new ArticleNormal();
                article.UserName = data.Username;
                article.Title = data.Title;
                article.Main = data.Main;
                article.ArticlecategoryId = data.ArticlecategoryId;
                article.IsFree = data.IsFree;
                article.IsPush = data.IsPush;
                article.InitDate = DateTime.Now;
                db.ArticleNormals.Add(article);
                db.SaveChanges();
                return Ok(new {stats = "success"});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        /// <summary>
        /// 找到作者的所有文章
        /// </summary>
        /// <param name="username">作者會員帳號</param>
        /// <param name="ispush">是否發布</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserAllArticleNormal(string username , bool ispush)
        {
            var data = from a in db.ArticleNormals
            where(a.UserName == username &
                  a.IsPush == ispush)
            select a;
            if (data == null)
            {
                return NotFound();
            }
            ArrayList arrayList = new ArrayList();
            foreach (var str in data.ToList())
            {
                var result = new
                {
                    str.ID,
                    str.Articlecategory.Name,
                    str.Title,
                    str.Main,
                    str.IsFree,
                    str.Lovecount,
                    str.InitDate
                };
                arrayList.Add(result);
            }

            return Ok(arrayList);
        }
        /// <summary>
        /// 刪除作者的文章
        /// </summary>
        /// <param name="artID">要刪除的文章ID</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteArticleNormal(int artID)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artID);
            if (data == null)
            {
                return Ok(new {status = "文章ID錯誤"});
            }

            db.ArticleNormals.Remove(data);
            db.SaveChanges();
            return Ok(new {status = "sucess"});
        }
        /// <summary>
        /// 更改一般文章
        /// </summary>
        /// <param name="artID">文章的ID</param>
        /// <param name="data">更改過後的資料</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult EditArticleNormal(int artID, DataArticleNormal data)
        {
            var havedata = db.ArticleNormals.FirstOrDefault(m => m.ID == artID);
            if (havedata == null)
            {
                return Ok(new {status = "文章ID錯誤"});
            }

            var editdata = from q in db.ArticleNormals
                where (q.ID == artID)
                select q;
            foreach (var q in editdata)
            {
                q.Title = data.Title;
                q.Main = data.Main;
                q.ArticlecategoryId = data.ArticlecategoryId;
                q.IsFree = data.IsFree;
                q.IsPush = data.IsPush;
               
            }

            db.SaveChanges();
            return Ok(new {status = "suceess"});
        }
        /// <summary>
        /// 一般文章按愛心
        /// </summary>
        /// <param name="artid">文章id</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult AddLoveArticleNormal(int artid, bool putlove)
        {
            var data = from q in db.ArticleNormals
                where (q.ID == artid)
                select q;
            if (data == null)
            {
                return NotFound();
            }

            int lovecount = db.ArticleNormals.FirstOrDefault(m => m.ID == artid).Lovecount;
            if (putlove == true)
            {
                lovecount++;
                foreach (var str in data)
                {
                    str.Lovecount = lovecount;
                }

                db.SaveChanges();
            }
            else
            {
                lovecount--;
                foreach (var str in data)
                {
                    str.Lovecount = lovecount;
                }
                db.SaveChanges();
            }

            return Ok(new { status = "sucess" });
        }
        /// <summary>
        /// 取得一般文章頁面所需資訊
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticleNormal(int artid)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artid);
            if (data == null)
            {
                return NotFound();
            }

            var Art_title = data.Title;
            var Art_main = data.Main;
            var Art_message = data.MessageNormals.ToList();
            var Art_initdate = data.InitDate;
            var Art_ispush = data.IsPush;
            var Art_isfree = data.IsFree;
           
            
            ArrayList messageArrayList = new ArrayList();
            ArrayList remessageArrayList = new ArrayList();
            foreach (var str in Art_message)
            {

                var mdata = new
                {
                    messageid = str.Id,
                    messagemain = str.Main,
                    messageinitdate = str.InitDate,
                };
                messageArrayList.Add(mdata);
                var rmessagedata = str.R_MessageNormals.ToList();
                foreach (var rstr in rmessagedata)
                {
                    var rdata = new
                    {
                        remessageid = rstr.Id,
                        remessagemain = rstr.Main,
                        remessageinitdate = rstr.InitDate,
                    };
                    remessageArrayList.Add(rdata);
                }
            }
            return Ok(new
            {
                Art_title,
                Art_main,
                Art_initdate,
                Art_ispush,
                Art_isfree,
                messageArrayList,
                remessageArrayList,
            });
        }
        /// <summary>
        /// 一般文章的留言
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <param name="username">留言者帳號</param>
        /// <param name="messagemain">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddmessageArticleNrmal(int artid, string username,string messagemain)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artid);
            if (data == null)
            {
                return NotFound();
            }
            MessageNormal message = new MessageNormal();
            message.ArticleNorId = artid;
            message.UserName = username;
            message.Main = messagemain;
            message.InitDate = DateTime.Now;
            db.MessageNormals.Add(message);
            db.SaveChanges();
            return Ok(new {status = "sucess"});
        }
        /// <summary>
        /// 回覆一般文章留言
        /// </summary>
        /// <param name="messageid">留言ID</param>
        /// <param name="messagemain">回覆留言的內容</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Addremessage(int messageid, string messagemain)
        {
            var data = db.MessageNormals.FirstOrDefault(m => m.Id == messageid);
            if (data == null)
            {
                return NotFound();
            }
            R_MessageNormal rMessage = new R_MessageNormal();
            rMessage.MessageNorId = messageid;
            rMessage.Main = messagemain;
            rMessage.InitDate = DateTime.Now;
            db.R_MessageNormals.Add(rMessage);
            db.SaveChanges();
            return Ok(new {status = "sucess"});
        }
        /// <summary>
        /// 找到一般文章所有的留言
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Getmessage")]
        public IHttpActionResult Getmessage(int artid )
        {
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artid);
            if (data == null)
            {
                return NotFound();
            }

            var message = data.MessageNormals.ToList();
            ArrayList resultList = new ArrayList();
            foreach (var str in message)
            {
                var result = new
                {
                    str.Id,
                    str.Main,
                    str.InitDate
                };
                resultList.Add(result);
            }

            return Ok(resultList);
        }
        /// <summary>
        /// 取得此留言回覆
        /// </summary>
        /// <param name="messageid">留言id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Getrmessage(int messageid)
        {
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageid);
            if (data == null)
            {
                return NotFound();
            }

            var message = data.R_MessageNormals.ToList();
            ArrayList resultList = new ArrayList();
            foreach (var str in message)
            {
                var result = new
                {
                    str.Id,
                    str.Main,
                    str.InitDate
                };
                resultList.Add(result);
            }

            return Ok(resultList);
        }
        /// <summary>
        /// 收藏一般文章
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <param name="memberid">收藏者的ID</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Collectarticle(int artid, int memberid)
        {
            var datArticle = db.ArticleNormals.FirstOrDefault(x => x.ID == artid);

            if (datArticle == null)
            {
                return NotFound();
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == memberid);
            member.ArticleNormals.Add(datArticle);
            db.SaveChanges();
            return Ok(new { status = "sucess" });

        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userid">會員ID</param>
        /// <param name="articleid">文章ID</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Deletecollect(int userid, int articleid)
        {
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == articleid);
            if (data == null)
            {
                return NotFound();
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == userid);
            member.ArticleNormals.Remove(data);
            db.SaveChanges();
            return Ok(new { status = "sucess" });
        }
        /// <summary>
        /// 取得會員收藏的一般文章
        /// </summary>
        /// <param name="memberid">會員ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllcollectart(int memberid)
        {
            var data = db.Members.FirstOrDefault(x => x.ID == memberid);
            var art = data.ArticleNormals.ToList();
            ArrayList artlList = new ArrayList();
            foreach (var str in art)
            {
                var result = new
                {
                    str.ID,
                    str.Articlecategory.Name,
                    str.UserName,
                    str.Title,
                };
                artlList.Add(result);
            }

            return Ok(artlList);
        }
    }
}
