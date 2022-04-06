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

            var authorname = checkusername.Name;
            var authorPic = checkusername.PicName + "." + checkusername.FileName;
            var checkArt = username +DateTime.Now.ToFileTime();
            ArticleNormal article = new ArticleNormal();
            article.UserName = data.userName;
            article.AuthorName = authorname;
            article.AuthorPic = authorPic;
            article.Introduction = data.introduction;
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
        /// <param name="isPush">是否發布(判斷是否在草稿)</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserAllArticleNormal( bool isPush,int nowpage,int showcount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = from a in db.ArticleNormals
            where(a.UserName == username &
                  a.IsPush == isPush)
            select a;
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有文章"
                });
            }
            List<NormalMessageCount> arrayList = new List<NormalMessageCount>();
            foreach (var str in data.ToList())
            {
                NormalMessageCount newartary = new NormalMessageCount();
                newartary.artId= str.ID;
                newartary.username = str.UserName;
                newartary.author = str.AuthorName;
                newartary.authorPic = str.AuthorPic;
                newartary.introduction = str.Introduction;
                newartary.title = str.Title;
                newartary.artArtlog = str.Articlecategory.Name;
                newartary.articlecategoryId = str.ArticlecategoryId;
                newartary.isFree = str.IsFree;
                newartary.messageCount = str.MessageNormals.Count;
                newartary.lovecount = str.Lovecount;
                newartary.artInitDate = str.InitDate;

                arrayList.Add(newartary);
            }

            var total = arrayList.Count;
            if (nowpage == 1)
            {
                var outPutData = arrayList.OrderByDescending(x => x.artInitDate).Take(showcount);
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
                var outPutData = arrayList.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
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
            var editdata = from q in db.ArticleNormals
                where (q.ID == artId)
                select q;
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
            var data = from q in db.ArticleNormals
                where (q.ID == artId)
                select q;
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
            }

            int lovecount = db.ArticleNormals.FirstOrDefault(m => m.ID == artId).Lovecount;
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
                success=true,
                lovecount=lovecount
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
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="查無此文章"
                });
            }

            var Art_title = data.Title;
            var Art_Info = data.Introduction;
            var Art_main = data.Main;
            //var Art_message = data.MessageNormals.ToList();
            var Art_initdate = data.InitDate;
            var Art_ispush = data.IsPush;
            var Art_isfree = data.IsFree;
            var lovenum = data.Lovecount;
            var username = data.UserName;
            var userData = db.Members.FirstOrDefault(x => x.UserName == username);
            var authorPic = userData.PicName + "." + userData.FileName;
            var author = userData.Name;

            var artlogid = data.ArticlecategoryId;
            
            //ArrayList messageArrayList = new ArrayList();
           
            //foreach (var str in Art_message)
            //{
            //    ArrayList remessageArrayList = new ArrayList();
            //    var rmessagedata = str.R_MessageNormals.ToList();
            //    foreach (var rstr in rmessagedata)
            //    {
            //        var rdata = new
            //        {
            //            reMessageId = rstr.Id,
            //            author = author,
            //            authorPic = authorPic,
            //            reMessageMain = rstr.Main,
            //            reMessageInitDate = rstr.InitDate,
            //        };
            //        remessageArrayList.Add(rdata);
            //    }

            //    var picname = str.Members.PicName + "." + str.Members.FileName;
            //    var mdata = new
            //    {
            //        messageId = str.Id,
            //        messageMember = str.Members.Name,
            //        messageMemberPic = picname,
            //        messageMain = str.Main,
            //        messageInitDate = str.InitDate,
            //        reMessageArrayList = remessageArrayList
            //    };
            //    messageArrayList.Add(mdata);
                
                
            //}
            return Ok(new
            {
                success = true,
                data =new{
                    username=username,
                    author= author,
                    authorPic = authorPic,
                    title = Art_title,
                    main = Art_main,
                    introduction=Art_Info,
                    articlecategoryId=artlogid,
                    artInitDate =Art_initdate,
                    isFree = Art_isfree,
                    isPush = Art_ispush,
                    lovecount= lovenum,
                    /*messageArrayList*/}
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
            var data = db.ArticleNormals.FirstOrDefault(m => m.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var checkusername = data.UserName;
            var jwtUsrname = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkusername != jwtUsrname)
            {
                return Ok(new
                {
                    suceess = false,
                    message = "你沒有權限"
                });
            }
            var Art_title = data.Title;
            var Art_info = data.Introduction;
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
                    introduction = Art_info,
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
            var data = db.MessageNormals.Where(x => x.ArticleNorId == artId).ToList();
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此文章"
                });
            }

            var userName = db.ArticleNormals.FirstOrDefault(x => x.ID == artId).UserName;
            var userData = db.Members.FirstOrDefault(x => x.UserName == userName);
            var author = userData.Name;
            var authorPic = userData.PicName + "." + userData.FileName;
            List<MessageList> arrayList = new List<MessageList>();

            foreach (var str in data)
            {
                List<MessageList.RMG> reList = new List<MessageList.RMG>();
                var picName = str.Members.PicName + "." + str.Members.FileName;
                var remessageDate = str.R_MessageNormals.ToList();
                foreach (var rstr in remessageDate)
                {
                    MessageList.RMG remessage = new MessageList.RMG();
                    remessage.reMessageId = rstr.Id;
                    remessage.userName = userName;
                    remessage.author = author;
                    remessage.authorPic = authorPic;
                    remessage.reMessageMain = rstr.Main;
                    remessage.reMessageInitDate = rstr.InitDate;
                    reList.Add(remessage);
                }

                MessageList array = new MessageList();
                array.messageId = str.Id;
                array.messageUserName = str.Members.UserName;
                array.messageMember = str.Members.Name;
                array.messageMemberPic = picName;
                array.messageMain = str.Main;
                array.messageInitDate = str.InitDate;
                array.reMessageData = reList;
                arrayList.Add(array);

            }

            var total = arrayList.Count;
            if (nowpage == 1)
            {
                var result = arrayList.OrderByDescending(x => x.messageInitDate).Take(showcount);
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
                var result = arrayList.OrderByDescending(x => x.messageInitDate).Skip(page).Take(showcount);
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
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }
            var userName = data.UserName;
          
            var userPic = data.Members.PicName + "." + data.Members.FileName;
            var main = data.Main;
            var initDate = data.InitDate;
            return Ok(new
            {
                success = true,
                messageMember = userName,
                messageMemberPic = userPic,
                messaageMain = main,
                messaageIniteDate = initDate
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
            var data = db.MessageNormals.FirstOrDefault(x => x.Id == messageId);
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
                    reMessageId = str.Id,
                    reMessageMain = str.Main,
                    reMessageInitDate = str.InitDate
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
            var data = db.Members.FirstOrDefault(x => x.ID == memberid);
            var art = data.ArticleNormals.ToList();
            List<NormalArticles> arrayList = new List<NormalArticles>();
            foreach (var str in art)
            {

                NormalArticles newartary = new NormalArticles();
                newartary.artId = str.ID;
                newartary.username = str.UserName;
                newartary.introduction = str.Introduction;
                newartary.title = str.Title;
                newartary.artArtlog = str.Articlecategory.Name;
                newartary.articlecategoryId = str.ArticlecategoryId;
                newartary.isFree = str.IsFree;
                newartary.lovecount = str.Lovecount;
                newartary.artInitDate = str.InitDate;

                arrayList.Add(newartary);

            }

            int total = arrayList.Count;
            if (nowpage == 1)
            {
                var result = arrayList.OrderByDescending(x => x.artInitDate).Take(showcount);
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

                var result = arrayList.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }

        }
        /// <summary>
        /// 依照類別取得相關一般文章(前四筆)
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
            var data = from q in db.ArticleNormals
                where q.ArticlecategoryId == articlecategoryId && q.IsPush == true
                select q;
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有相關文章"
                });
            }
            List<NewNormalArticle> arrayList = new List<NewNormalArticle>();

            foreach (var str in data.ToList())
            {

                NewNormalArticle newartary = new NewNormalArticle();
                newartary.artId = str.ID;
                newartary.username = str.UserName;
                newartary.author = str.AuthorName;
                newartary.authorPic = str.AuthorPic;
                newartary.introduction = str.Introduction;
                newartary.title = str.Title;
                newartary.artArtlog = str.Articlecategory.Name;
                newartary.articlecategoryId = str.ArticlecategoryId;
                newartary.isFree = str.IsFree;
                newartary.lovecount = str.Lovecount;
                newartary.artInitDate = str.InitDate;

                arrayList.Add(newartary);

            }

            int total = arrayList.Count;
            if (nowpage == 1)
            {
                var outPut = arrayList.OrderByDescending(x => x.artInitDate).Take(showcount);
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
                var outPut = arrayList.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
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

            var q = from p in db.MessageNormals
                    where p.Id == messageId
                    select p;
            foreach (var p in q)
            {
                p.Main = main;
            }

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

            var q = from p in db.R_MessageNormals
                    where p.Id == reMessageId
                    select p;
            foreach (var p in q)
            {
                p.Main = main;
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改回覆"
            });
        }
    }
}
