using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
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

            ArticleNormal article = new ArticleNormal();
            article.UserName = data.userName;
            article.Introduction = data.introduction;
            article.Title = data.title;
            article.Main = data.main;
            article.ArticlecategoryId = data.articlecategoryId;
            article.IsFree = data.isFree;
            article.IsPush = data.isPush;
            article.InitDate = DateTime.Now;
            article.CheckArticle = username + DateTime.Now.ToFileTime();
            db.ArticleNormals.Add(article);
            db.SaveChanges();
        
            return Ok(new
            {
                success=true,
                message = "已新增文章",
                artId = article.ID
            });
        }
        /// <summary>
        /// 找到作者自己的所有一般文章
        /// </summary>
        /// <param name="isPush">是否發布(判斷是否在草稿)</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserAllArticleNormal( bool isPush,int nowpage,int showcount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var memeberData = db.Members.FirstOrDefault(m => m.UserName == username);
            if ( memeberData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            //var data = db.ArticleNormals.Where(m => m.UserName == username).Where(m => m.IsPush == isPush).Select(x =>
            //    new
            //    {
            //        artId=x.ID,
            //        username=x.UserName,
            //        author=x.AuthorName,
            //        authorPic=x.AuthorPic,
            //        introduction=x.Introduction,
            //        title=x.Title,
            //        artArtlog=x.Articlecategory.Name,
            //        articlecategoryId=x.ArticlecategoryId,
            //        isFree=x.IsFree,
            //        messageCount=x.MessageNormals.Count,
            //        lovecount=x.Lovecount,
            //        artInitDate=x.InitDate

            //    }).ToList();

            var data = db.ArticleNormals.Where(m => m.UserName == username && m.IsPush == isPush).
                Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
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
                }).Select(x =>
                new
                {
                    artId=x.artId,
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

           

            var total = data.Count;
            if (nowpage == 1)
            {
                var outPutData = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPutData
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var outPutData = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
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
        /// <param name="artId">要刪除的文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/DeleteArticleNormal")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteArticleNormal(int artId)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            var dataUser = data.UserName;
            if (dataUser != username)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }
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
        /// <param name="artId">文章的ID</param>
        /// <param name="data">更改過後的資料</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult EditArticleNormal(int artId, DataArticleNormal data)
        {
            var havedata = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (havedata == null)
            {
                return Ok(new
                {
                    success=false,
                    message = "文章ID錯誤"
                });
            }

            var checkuserName = havedata.UserName;
            var jwtUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkuserName != jwtUserName)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            var editdata = db.ArticleNormals.Where(x => x.ID == artId);
            foreach (var q in editdata)
            {
                q.Title = data.title;
                q.Introduction = data.introduction;
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
        /// <param name="artId">文章id</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult AddLoveArticleNormal(int artId, bool putlove)
        {
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
            }

            if (putlove == true)
            {
                data.Lovecount++;

                db.SaveChanges();
            }
            else
            {
                data.Lovecount--;
                db.SaveChanges();
            }

            return Ok(new
            {
                success=true,
                lovecount= data.Lovecount
        });
        }
        /// <summary>
        /// 取得一般文章頁面所需資訊
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticleNormal(int? artId)
        {
            //var data = db.ArticleNormals.Where(m => m.ID == artId).Select(x=>new
            //{
            //    username = x.UserName,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    introduction = x.Introduction,
            //    title = x.Title,
            //    articlecategoryId = x.ArticlecategoryId,
            //    main=x.Main,
            //    isFree = x.IsFree,
            //    isPush=x.IsPush,
            //    messageCount = x.MessageNormals.Count,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).FirstOrDefault();
            var data = db.ArticleNormals.Join(
                db.Members,
                c => c.UserName,
                o => o.UserName,
                (c, o) => new
                {
                    artId = c.ID,
                    userName = c.UserName,
                    authorName = o.Name,
                    authorPic = o.PicName + "." + o.FileName,
                    introduction = c.Introduction,
                    title = c.Title,
                    articlecategoryId = c.ArticlecategoryId,
                    artArtlog = c.Articlecategory.Name,
                    main = c.Main,
                    isFree = c.IsFree,
                    isPush = c.IsPush,
                    messageCount = c.MessageNormals.Count,
                    lovecount = c.Lovecount,
                    artInitDate = c.InitDate
                }).Where(x => x.artId == artId).Select(x=>new
            {
                username = x.userName,
                author = x.authorName,
                authorPic = x.authorPic,
                introduction = x.introduction,
                title = x.title,
                articlecategoryId = x.articlecategoryId,
                artArtlog = x.artArtlog,
                main = x.main,
                isFree = x.isFree,
                isPush = x.isPush,
                messageCount = x.messageCount,
                lovecount = x.lovecount,
                artInitDate = x.artInitDate
                }).FirstOrDefault();
            
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="查無此文章"
                });
            }

            return Ok(new
            {
                success = true,
                data =data
        });
        }
        /// <summary>
        /// 編輯用一般文章資訊
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/GetNormalArticle")]
        [ResponseType(typeof(NormalArticleOutPutForEdit))]
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetNormalArticleForEdit(int? artId)
        {
            var data = db.ArticleNormals.Where(m => m.ID == artId).Select(x=>new
            {
                title = x.Title,
                introduction = x.Introduction,
                main = x.Main,
                articlecategoryId = x.ArticlecategoryId,
                artInitDate = x.InitDate,
                isFree = x.IsFree,
                isPush = x.IsPush,
            }).FirstOrDefault();
            
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var checkusername = db.ArticleNormals.FirstOrDefault(x=>x.ID==artId).UserName;
            var jwtUsrname = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkusername != jwtUsrname)
            {
                return Ok(new
                {
                    suceess = false,
                    message = "你沒有權限"
                });
            }
          

            return Ok(new
            {
                success = true,
                data = data
                
            });
        }
        /// <summary>
        /// 一般文章的留言
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <param name="main">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddmessageArticleNrmal(int artId, string main)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此文章"
                });
            }
            MessageNormal message = new MessageNormal();
            message.MemberID = userId;
            message.ArticleNorId = artId;
            message.UserName = username;
            message.Main = main;
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
        /// <param name="messageId">留言ID</param>
        /// <param name="main">回覆留言的內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Addremessage(int messageId, string main)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.MessageNormals.FirstOrDefault(m => m.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此留言"
                });
            }
            var dataArtID = data.ArticleNorId;
            var datausername = db.ArticleNormals.FirstOrDefault(x => x.ID == dataArtID).UserName;
            if (username != datausername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }
            
            R_MessageNormal rMessage = new R_MessageNormal();
            rMessage.MessageNorId = messageId;
            rMessage.Main = main;
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
        /// <param name="artId">文章ID</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/GetAllmessage")]
        [HttpGet]
        public IHttpActionResult Getmessage(int artId ,int nowpage,int showcount)
        {
            //var data = db.MessageNormals.Where(m => m.ArticleNorId == artId).Select(x=>new
            //{
            //    messageId = x.Id,
            //    messageUserName = x.Members.UserName,
            //    messageMember = x.Members.Name,
            //    messageMemberPic = x.Members.PicName + "." + x.Members.FileName,
            //    messageMain = x.Main,
            //    messageInitDate = x.InitDate,
            //    reMessageData = x.R_MessageNormals.Select(y => new
            //    {
            //        reMessageId = y.Id,
            //        userName = y.MessageNormals.ArticleNormals.UserName,
            //        author = y.MessageNormals.ArticleNormals.AuthorName,
            //        authorPic = y.MessageNormals.ArticleNormals.AuthorPic,
            //        reMessageMain = y.Main,
            //        reMessageInitDate = y.InitDate
            //    })
            //}).ToList();
            var data = db.MessageNormals.Where(m => m.ArticleNorId == artId).Select(x => new
            {
                messageId = x.Id,
                messageUserName = x.Members.UserName,
                messageMember = x.Members.Name,
                messageMemberPic = x.Members.PicName + "." + x.Members.FileName,
                messageMain = x.Main,
                messageInitDate = x.InitDate,
                reMessageData = x.R_MessageNormals.
                    Join(db.Members,a=>a.MessageNormals.ArticleNormals.UserName,b=>b.UserName,(a,b)=>new
                    {
                        reMessageId = a.Id,
                        userName = b.UserName,
                        author = b.Name,
                        authorPic = b.PicName+"."+b.FileName,
                        reMessageMain = a.Main,
                        reMessageInitDate = a.InitDate
                    }).Select(y => new
                {
                    reMessageId = y.reMessageId,
                    userName = y.userName,
                    author = y.author,
                    authorPic = y.authorPic,
                    reMessageMain = y.reMessageMain,
                    reMessageInitDate = y.reMessageInitDate
                })
            }).ToList();
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
            }


            var total = data.Count;
            if (nowpage == 1)
            {
                var result = data.OrderByDescending(x => x.messageInitDate).Take(showcount);
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
                var result = data.OrderByDescending(x => x.messageInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
        }
        /// <summary>
        /// 取得一般文章一筆留言(含大頭貼))
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/Getmessage")]
        [HttpGet]
        [ResponseType(typeof(OutPutMessage))]
        public IHttpActionResult GetOneMessage(int messageId)
        {
            var data = db.MessageNormals.Where(m => m.Id == messageId).Select(y=>new
            {
                messageUserName = y.UserName,
                messageMember = y.Members.Name,
                messageMemberPic = y.Members.PicName + "." + y.Members.FileName,
                messageMain = y.Main,
                messageInitDate = y.InitDate
            }).FirstOrDefault();
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }


            return Ok(new
            {
                success = true,
                data=data
            });
        }
        /// <summary>
        /// 作者刪除留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMessage(int messageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
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
        /// 作者刪除回覆
        /// </summary>
        /// <param name="reMessageId">回覆ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/ArticleNormal/DeleteReMessage")]
        [JwtAuthFilter]
        public IHttpActionResult DeleteReMessage(int reMessageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.R_MessageNormals.FirstOrDefault(x => x.Id == reMessageId);
            var messaageId = data.MessageNorId;
            var dataId = db.MessageNormals.FirstOrDefault(x => x.Id == messaageId).ArticleNorId;
            var artmember = db.ArticleNormals.FirstOrDefault(x => x.ID == dataId).UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            db.R_MessageNormals.Remove(data);
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
        /// <param name="messageId">留言id</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(OutPutReMessage))]
        public IHttpActionResult Getrmessage(int messageId)
        {
            var data = db.R_MessageNormals.Where(x => x.Id == messageId).
                Join(db.Members,a=>a.MessageNormals.ArticleNormals.UserName,b=>b.UserName,(a,b)=>new
                {
                    reMessageId = a.Id,
                    userName = b.UserName,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    reMessageMain = a.Main,
                    reMessageInitDate = a.InitDate
                }).Select(y=>new
            {
                reMessageId = y.reMessageId,
                userName = y.userName,
                author = y.author,
                authorPic = y.authorPic,
                reMessageMain = y.reMessageMain,
                reMessageInitDate = y.reMessageInitDate
                }).ToList();
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="無此留言"
                });
            }

            

            return Ok(new
            {
                success=true,
                data=data
            });
        }
        /// <summary>
        /// 收藏一般文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Collectarticle(int artId)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var datArticle = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);

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
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/Deletecollect")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult Deletecollect(int artId)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.ArticleNormals.FirstOrDefault(x => x.ID == artId);
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
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int nowpage,int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            //var data = db.Members.FirstOrDefault(m => m.ID == memberid).ArticleNormals.Select(x=>new
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
            var data = db.Members.FirstOrDefault(m => m.ID == memberid).ArticleNormals
                .Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
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

            int total = data.Count;
            if (nowpage == 1)
            {
                var result = data.OrderByDescending(x => x.artInitDate).Take(showcount);
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
                //排序依照日期

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
        /// 依照類別取得相關一般文章
        /// </summary>
        /// <param name="articlecategoryId">文章類別ID</param>
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArtlogNormalArticle(int articlecategoryId,int nowpage,int showcount)
        {
            //var data = db.ArticleNormals.Where(x => x.ArticlecategoryId == articlecategoryId).Where(x=>x.IsPush==true)
            //    .OrderByDescending(x => x.InitDate).ToList();
            //var data = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId)
            //    .Where(m => m.IsPush == true).Select(x=>new
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
            //        artInitDate = x.InitDate
            //    }).ToList();
            var data = db.ArticleNormals.Where(m => m.ArticlecategoryId == articlecategoryId && m.IsPush == true)
                .Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
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
                }).ToList();
                //Select(x => new
                //{
                //    artId = x.artId,
                //    username = x.userName,
                //    author = x.authorName,
                //    authorPic = x.authorPic,
                //    introduction = x.introduction,
                //    title = x.title,
                //    articlecategoryId = x.articlecategoryId,
                //    artArtlog = x.artArtlog,
                //    main = x.main,
                //    isFree = x.isFree,
                //    messageCount = x.messageCount,
                //    lovecount = x.lovecount,
                //    artInitDate = x.artInitDate
                //}).ToList();
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有相關文章"
                });
            }
            

            int total = data.Count;
            if (nowpage == 1)
            {
                var outPut = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total=total,
                    data = outPut
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var outPut = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = outPut
                });
            }
            
        }

        /// <summary>
        /// 修改留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="main">修改後內容</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/EditMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditMessage(int messageId, string main)
        {
            var messageData = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
            if (messageData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }
            var memberUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var checkUserName = messageData.UserName;
            if (checkUserName != memberUserName)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            messageData.Main = main;
            //var q = db.MessageNormals.Where(x => x.Id == messageId);
            //foreach (var p in q)
            //{
            //    p.Main = main;
            //}

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改留言"
            });
        }
        /// <summary>
        /// 修改回覆
        /// </summary>
        /// <param name="reMessageId">回覆的ID</param>
        /// <param name="main">修改的內容</param>
        /// <returns></returns>
        [Route("api/ArticleNormal/EditReMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditReMessage(int reMessageId, string main)
        {
            var reData = db.R_MessageNormals.FirstOrDefault(x => x.Id == reMessageId);
            if (reData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此回覆"
                });
            }

            var checkMessageId = reData.MessageNorId;
            var checkArticleId = db.MessageNormals.FirstOrDefault(x => x.Id == checkMessageId).ArticleNorId;
            var checkUserName = db.ArticleNormals.FirstOrDefault(x => x.ID == checkArticleId).UserName;
            var memberUserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUserName != memberUserName)
            {
                return Ok(new
                {
                    suceess = false,
                    message = "你沒有權限"
                });
            }

            reData.Main = main;
            //var q = db.R_MessageNormals.Where(x => x.Id == reMessageId);
            
            //foreach (var p in q)
            //{
            //    p.Main = main;
            //}

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改回覆"
            });
        }
    }
}
