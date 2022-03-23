using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using testdatamodel.JWT;
using testdatamodel.listclass;
using testdatamodel.Models;
using testdatamodel.PutData;
using testdatamodel.Swagger;

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
        [JwtAuthFilter]
        public IHttpActionResult CreatArticleNormal(DataArticleNormal data)
        {
            var username = data.userName;
            var checkusername = db.Members.FirstOrDefault(m => m.UserName == username);
            if (checkusername == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有此帳號"
                });
            }

            var checkArt = username +DateTime.Now.ToFileTime();
            ArticleNormal article = new ArticleNormal();
            article.UserName = data.userName;
            article.Title = data.title;
            article.Main = data.main;
            article.ArticlecategoryId = data.articlecategoryId;
            article.IsFree = data.isFree;
            article.IsPush = data.isPush;
            article.InitDate = DateTime.Now;
            article.CheckArticle = checkArt;
            db.ArticleNormals.Add(article);
            db.SaveChanges();
            var artId = db.ArticleNormals.FirstOrDefault(x => x.CheckArticle == checkArt).ID;
            return Ok(new
            {
                success=true,
                message = "已新增文章",
                artId = artId
            });
        }
        /// <summary>
        /// 找到作者自己的所有一般文章
        /// </summary>
        /// <param name="ispush">是否發布(判斷是否在草稿)</param>
        /// <param name="nowPage">現在頁數(預設1)</param>
        /// <param name="showNum">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserAllArticleNormal( bool ispush,int nowPage,int showNum)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = from a in db.ArticleNormals
            where(a.UserName == username &
                  a.IsPush == ispush)
            select a;
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有文章"
                });
            }
            List<NormalArticles> arrayList = new List<NormalArticles>();
            foreach (var str in data.ToList())
            {
                NormalArticles newartary = new NormalArticles();
                newartary.ArticleID = str.ID;
                newartary.UserName = str.UserName;
                newartary.Title = str.Title;
                newartary.Articlecategory = str.Articlecategory.Name;
                newartary.Isfree = str.IsFree;
                newartary.Lovecount = str.Lovecount;
                newartary.InitDateTime = str.InitDate;

                arrayList.Add(newartary);
            }

            var total = arrayList.Count;
            if (nowPage == 1)
            {
                var outPutData = arrayList.OrderByDescending(x => x.InitDateTime).Take(showNum);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPutData
                });
            }
            else
            {
                var page = (nowPage - 1) * showNum;
                var outPutData = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showNum);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPutData
                });
            }

            
        }
        /// <summary>
        /// 刪除作者的文章
        /// </summary>
        /// <param name="artID">要刪除的文章ID</param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteArticleNormal(int artID)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artID);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message = "文章ID錯誤"
                });
            }

            db.ArticleNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message = "已刪除"
            });
        }
        /// <summary>
        /// 更改一般文章
        /// </summary>
        /// <param name="artID">文章的ID</param>
        /// <param name="data">更改過後的資料</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult EditArticleNormal(int artID, DataArticleNormal data)
        {
            var havedata = db.ArticleNormals.FirstOrDefault(m => m.ID == artID);
            if (havedata == null)
            {
                return Ok(new
                {
                    success=false,
                    message = "文章ID錯誤"
                });
            }

            var editdata = from q in db.ArticleNormals
                where (q.ID == artID)
                select q;
            foreach (var q in editdata)
            {
                q.Title = data.title;
                q.Main = data.main;
                q.ArticlecategoryId = data.articlecategoryId;
                q.IsFree = data.isFree;
                q.IsPush = data.isPush;
               
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message="已完成編輯"
            });
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
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
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

            return Ok(new
            {
                success=true
            });
        }
        /// <summary>
        /// 取得一般文章頁面所需資訊
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticleNormal(int? artid)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artid);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="查無此文章"
                });
            }

            var Art_title = data.Title;
            var Art_main = data.Main;
            var Art_message = data.MessageNormals.ToList();
            var Art_initdate = data.InitDate;
            var Art_ispush = data.IsPush;
            var Art_isfree = data.IsFree;

            var artlogid = data.ArticlecategoryId;
            
            ArrayList messageArrayList = new ArrayList();
            ArrayList remessageArrayList = new ArrayList();
            foreach (var str in Art_message)
            {

                var mdata = new
                {
                    messageId = str.Id,
                    messageMain = str.Main,
                    messageInitDate = str.InitDate,
                };
                messageArrayList.Add(mdata);
                var rmessagedata = str.R_MessageNormals.ToList();
                foreach (var rstr in rmessagedata)
                {
                    var rdata = new
                    {
                        reMessageId = rstr.Id,
                        reMessageMain = rstr.Main,
                        reMessageInitDate = rstr.InitDate,
                    };
                    remessageArrayList.Add(rdata);
                }
            }
            return Ok(new
            {
                success = true,
                data =new{
                    title = Art_title,
                    main = Art_main,
                    articlecategoryId=artlogid,
                    artInitDate =Art_initdate,
                    isFree = Art_ispush,
                    isPush = Art_isfree,
                    messageArrayList,
                    reMessageArrayList=remessageArrayList}
        });
        }
        /// <summary>
        /// 編輯用一般文章資訊
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/GetNormalArticle")]
        [ResponseType(typeof(NormalArticleOutPutForEdit))]
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetNormalArticleForEdit(int? artId)
        {
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var Art_title = data.Title;
            var Art_main = data.Main;
            var Art_message = data.MessageNormals.ToList();
            var Art_initdate = data.InitDate;
            var Art_ispush = data.IsPush;
            var Art_isfree = data.IsFree;

            var artlogid = data.ArticlecategoryId;

            return Ok(new
            {
                success = true,
                data = new
                {
                    title = Art_title,
                    main = Art_main,
                    articlecategoryId = artlogid,
                    artInitDate = Art_initdate,
                    isFree = Art_ispush,
                    isPush = Art_isfree,
                }
            });
        }
        /// <summary>
        /// 一般文章的留言
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <param name="messagemain">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddmessageArticleNrmal(int artid, string messagemain)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artid);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此文章"
                });
            }
            MessageNormal message = new MessageNormal();
            message.ArticleNorId = artid;
            message.UserName = username;
            message.Main = messagemain;
            message.InitDate = DateTime.Now;
            db.MessageNormals.Add(message);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message = "留言成功"
            });
        }
        /// <summary>
        /// 回覆一般文章留言
        /// </summary>
        /// <param name="messageid">留言ID</param>
        /// <param name="messagemain">回覆留言的內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Addremessage(int messageid, string messagemain)
        {
            var data = db.MessageNormals.FirstOrDefault(m => m.Id == messageid);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此留言"
                });
            }
            R_MessageNormal rMessage = new R_MessageNormal();
            rMessage.MessageNorId = messageid;
            rMessage.Main = messagemain;
            rMessage.InitDate = DateTime.Now;
            db.R_MessageNormals.Add(rMessage);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message = "已回覆"
            });
        }
        /// <summary>
        /// 找到一般文章所有的留言
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [Route("GetAllmessage")]
        [HttpGet]
        public IHttpActionResult Getmessage(int artid )
        {
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artid);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
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

            return Ok(new
            {
                success=true,
                data=resultList
            });
        }
        /// <summary>
        /// 取得一般文章一筆留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [Route("Getmessage")]
        [HttpGet]
        public IHttpActionResult GetOneMessage(int messageId)
        {
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }

            var main = data.Main;
            return Ok(new
            {
                success = true,
                messaageMain = main
            });
        }
        /// <summary>
        /// 作者刪除留言
        /// </summary>
        /// <param name="messageID">留言ID</param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMessage(int messageID)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageID);
            var dataId = data.ArticleNorId;
            var artmember = db.ArticleNormals.FirstOrDefault(x => x.ID == dataId).UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限刪除留言"
                });
            }

            db.MessageNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

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
                return Ok(new
                {
                    success = false,
                    message="無此留言"
                });
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

            return Ok(new
            {
                success=true,
                data=resultList
            });
        }
        /// <summary>
        /// 收藏一般文章
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Collectarticle(int artid)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var datArticle = db.ArticleNormals.FirstOrDefault(x => x.ID == artid);

            if (datArticle == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == memberid);
            member.ArticleNormals.Add(datArticle);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message = "以收藏"
            });

        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="articleid">文章ID</param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult Deletecollect(int articleid)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == articleid);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == userid);
            member.ArticleNormals.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message="以取消收藏"
            });
        }
        /// <summary>
        /// 取得會員收藏的一般文章
        /// </summary>
        /// <param name="Nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int Nowpage,int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Members.FirstOrDefault(x => x.ID == memberid);
            var art = data.ArticleNormals.ToList();
            List<NormalArticles> arrayList = new List<NormalArticles>();
            foreach (var content in art)
            {

                NormalArticles newartary = new NormalArticles();
                newartary.ArticleID = content.ID;
                newartary.UserName = content.UserName;
                newartary.Title = content.Title;
                newartary.Articlecategory = content.Articlecategory.Name;
                newartary.Lovecount = content.Lovecount;
                newartary.InitDateTime = content.InitDate;

                arrayList.Add(newartary);


            }

            int total = arrayList.Count;
            if (Nowpage == 1)
            {
                var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                int page = (Nowpage - 1) * showcount;
                //排序依照日期

                var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }

        }
    }
}
