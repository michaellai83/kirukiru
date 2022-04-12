using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testdatamodel.JWT;
using testdatamodel.listclass;
using testdatamodel.Models;
using testdatamodel.PutData;
using testdatamodel.Swagger;
using Member = testdatamodel.Models.Member;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 切切的文章
    /// </summary>
    public class ArticleController : ApiController
    {
        ProjectDb db = new ProjectDb();

        ///// <summary>
        ///// 添加切切文章
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[JwtAuthFilter]

        //public async Task<IHttpActionResult> AddArticle()
        //{
        //    //if (!this.Request.Content.IsMimeMultipartContent())
        //    //{
        //    //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    //}

        //    //var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pic");
        //    //var exists = Directory.Exists(root);
        //    //if (!exists)
        //    //{
        //    //    Directory.CreateDirectory("Pic");
        //    //}

        //    //try
        //    //{
        //    //    //var _MemberID = HttpContext.Current.Request.Form.GetValues("MemberID");
        //    //    //string MemberID = _MemberID[0];
        //    //    //int UserID = Convert.ToInt32(MemberID);
        //    //    var _username = HttpContext.Current.Request.Form.GetValues("userName"); //找到formdata 文字檔 key值為 username的values值
        //    //    string username = _username[0]; //value值
        //    //    var _title = HttpContext.Current.Request.Form.GetValues("title");
        //    //    string title = _title[0];
        //    //    var _Isfree = HttpContext.Current.Request.Form.GetValues("isFree");
        //    //    string Isfree = _Isfree[0];
        //    //    bool Is_Free = true;
        //    //    if (Isfree == "False")
        //    //    {
        //    //        Is_Free = false;
        //    //    }

        //    //    var _Introduction = HttpContext.Current.Request.Form.GetValues("introduction");
        //    //    string Introduction = _Introduction[0];
        //    //    var _ArticlecategoryId = HttpContext.Current.Request.Form.GetValues("articlecategoryId");
        //    //    string ArticlecategoryId = _ArticlecategoryId[0];
        //    //    var _IsPush = HttpContext.Current.Request.Form.GetValues("isPush");
        //    //    string IsPush = _IsPush[0];
        //    //    bool Is_Push = true;
        //    //    if (IsPush == "False")
        //    //    {
        //    //        Is_Push = false;
        //    //    }


        //    //    var _ArtMain = HttpContext.Current.Request.Form.GetValues("main");
        //    //    int artmaincount = 0;



        //    //    //下面是前置任務
        //    //    var _FirstMission = HttpContext.Current.Request.Form.GetValues("mission");


        //    //    var _finMission = HttpContext.Current.Request.Form.GetValues("auxiliary");
        //    //    var _finMissionMain = HttpContext.Current.Request.Form.GetValues("auxiliarymain");
        //    //    int finMissionCount = 0;


        //    //    int firstcount = 0;

        //    //    int ArtId = 0;

        //    //    var provider = new MultipartMemoryStreamProvider();
        //    //    await this.Request.Content.ReadAsMultipartAsync(provider);

        //    //    //var uploadResponse = new UploadResponse();
        //    //    foreach (var content in provider.Contents)
        //    //    {
        //    //        var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');

        //    //        if (KeyName.Contains("first"))
        //    //        {
        //    //            var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');

        //    //            if (filerealName.Equals(""))
        //    //            {
        //    //                return Ok(new
        //    //                {
        //    //                    success = false,
        //    //                    message = "請上傳首圖"
        //    //                });
        //    //            }
        //    //            string[] fileary = filerealName.Split('.');
        //    //            var fileBytes = await content.ReadAsByteArrayAsync();

        //    //            var filefirst = DateTime.Now.ToFileTime() + username;
        //    //            var fileName = filefirst + "." + fileary[1];

        //    //            var outputPath = Path.Combine(root, fileName);
        //    //            using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //    //            {
        //    //                await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //    //            }
        //    //            Article article = new Article();

        //    //            article.UserName = username;
        //    //            article.Title = title;
        //    //            article.IsFree = Is_Free;
        //    //            article.Introduction = Introduction;
        //    //            article.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
        //    //            article.IsPush = Is_Push;
        //    //            article.FirstPicName = filefirst;
        //    //            article.FirstPicFileName = fileary[1];
        //    //            article.InitDate = DateTime.Now;
        //    //            db.Articles.Add(article);
        //    //            db.SaveChanges();
        //    //            var result = db.Articles.FirstOrDefault(m => m.FirstPicName == filefirst);
        //    //            ArtId = result.ID;


        //    //        }
        //    //        else if (KeyName.Contains("sec"))
        //    //        {

        //    //            var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //    //            if (filerealName.Equals(""))
        //    //            {
        //    //                Firstmission firstmission = new Firstmission();
        //    //                firstmission.PicName = "";
        //    //                firstmission.PicFileName = "";
        //    //                firstmission.ArticleId = ArtId;
        //    //                firstmission.FirstItem = _FirstMission[firstcount];
        //    //                firstmission.InitDate = DateTime.Now;
        //    //                db.Firstmissions.Add(firstmission);
        //    //            }
        //    //            else
        //    //            {
        //    //                string[] fileary = filerealName.Split('.');
        //    //                var fileBytes = await content.ReadAsByteArrayAsync();

        //    //                var filefirst = DateTime.Now.ToFileTime() + username;
        //    //                var fileName = filefirst + "." + fileary[1];

        //    //                var outputPath = Path.Combine(root, fileName);
        //    //                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //    //                {
        //    //                    await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //    //                }

        //    //                Firstmission firstmission = new Firstmission();
        //    //                firstmission.PicName = filefirst;
        //    //                firstmission.PicFileName = fileary[1];
        //    //                firstmission.ArticleId = ArtId;
        //    //                firstmission.FirstItem = _FirstMission[firstcount];
        //    //                firstmission.InitDate = DateTime.Now;
        //    //                db.Firstmissions.Add(firstmission);
        //    //            }

        //    //            //db.SaveChanges();
        //    //            firstcount++;
        //    //        }
        //    //        else if (KeyName.Contains("third"))
        //    //        {
        //    //            var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //    //            if (filerealName.Equals(""))
        //    //            {
        //    //                ArticleMain articleMain = new ArticleMain();
        //    //                articleMain.PicName = "";
        //    //                articleMain.PicFileName = "";
        //    //                articleMain.ArticleId = ArtId;
        //    //                articleMain.Main = _ArtMain[artmaincount];
        //    //                articleMain.InDateTime = DateTime.Now;
        //    //                db.ArticleMains.Add(articleMain);
        //    //                //db.SaveChanges();
        //    //            }
        //    //            else
        //    //            {
        //    //                string[] fileary = filerealName.Split('.');
        //    //                var fileBytes = await content.ReadAsByteArrayAsync();

        //    //                var filefirst = DateTime.Now.ToFileTime() + username;
        //    //                var fileName = filefirst + "." + fileary[1];

        //    //                var outputPath = Path.Combine(root, fileName);
        //    //                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //    //                {
        //    //                    await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //    //                }

        //    //                ArticleMain articleMain = new ArticleMain();
        //    //                articleMain.PicName = filefirst;
        //    //                articleMain.PicFileName = fileary[1];
        //    //                articleMain.ArticleId = ArtId;
        //    //                articleMain.Main = _ArtMain[artmaincount];
        //    //                articleMain.InDateTime = DateTime.Now;
        //    //                db.ArticleMains.Add(articleMain);

        //    //                //db.SaveChanges();

        //    //            }
        //    //            artmaincount++;
        //    //        }
        //    //        else 
        //    //        {

        //    //        }

        //    //        //uploadResponse.Names.Add(fileName);
        //    //        //uploadResponse.FileNames.Add(outputPath);
        //    //        //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
        //    //    }
        //    //    if (_finMission.Length > 0)
        //    //    {
        //    //        foreach (var str in _finMission)
        //    //        {
        //    //            FinalMission finalMission = new FinalMission();
        //    //            finalMission.Title = str;
        //    //            finalMission.Main = _finMissionMain[finMissionCount];
        //    //            finalMission.InitDateTime = DateTime.Now;
        //    //            finalMission.ArticleId = ArtId;
        //    //            db.FinalMissions.Add(finalMission);
        //    //            finMissionCount++;
        //    //        }

        //    //    }

        //    //    var _Final = HttpContext.Current.Request.Form.GetValues("final");
        //    //    if (_Final != null)
        //    //    {
        //    //        foreach (var str in _Final)
        //    //        {
        //    //            Remark remark = new Remark();
        //    //            remark.Main = str;
        //    //            remark.InitTime = DateTime.Now;
        //    //            remark.ArticleId = ArtId;
        //    //            db.Remarks.Add(remark);

        //    //        }
        //    //        //db.SaveChanges();
        //    //    }
        //    //    db.SaveChanges();
        //    //    return Ok(new
        //    //    {
        //    //        success = true,
        //    //        message = "已新增文章"
        //    //    });
        //    //    //return this.Ok(uploadResponse);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //    //    {
        //    //        Content = new StringContent(e.Message)
        //    //    });
        //    //}

        //}

        //[HttpGet]
        //public IHttpActionResult GetArticle(string username)
        //{
        //    var data = db.Articles.Where(m => m.UserName == username).ToList();
        //}
        /// <summary>
        /// 添加切切文章
        /// </summary>
        /// <param name="dataArticle ">切切前端傳進來資料</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddArticle([FromBody] DataArticle dataArticle)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            //var username = dataArticle.memberUserName;
            if (string.IsNullOrWhiteSpace(dataArticle.firstPhoto) || string.IsNullOrWhiteSpace(dataArticle.title) || dataArticle.articlecategoryId == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "請上傳首圖和填寫標題和填寫分類"
                });
            }
            var artTitlePic = dataArticle.firstPhoto.Split('.');
            string titlePicName = artTitlePic[0];
            string titleFileName = artTitlePic[1];
            var memberdata = db.Members.FirstOrDefault(x => x.UserName == username);

            Article article = new Article();
            article.UserName = username;
            article.Title = dataArticle.title;
            article.FirstPicName = titlePicName;
            article.FirstPicFileName = titleFileName;
            article.Introduction = dataArticle.introduction;
            article.ArticlecategoryId = dataArticle.articlecategoryId;
            article.InitDate = DateTime.Now;
            article.IsFree = dataArticle.isFree;
            article.IsPush = dataArticle.isPush;
            article.Lovecount = 0;
            db.Articles.Add(article);
            db.SaveChanges();
            //存完直接回傳ID
            var artId = article.ID;


            foreach (var data in dataArticle.fArrayList)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.secPhoto))
                {
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.PicFileName = picFileName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);

                }
                else
                {
                    var picname = data.secPhoto.Split('.');
                    picName = picname[0];
                    picFileName = picname[1];
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.PicFileName = picFileName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);
                }

            }

            foreach (var data in dataArticle.mArrayList)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.thirdPhoto))
                {
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.PicFileName = picFileName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }
                else
                {
                    var kiruPhoto = data.thirdPhoto.Split('.');
                    picName = kiruPhoto[0];
                    picFileName = kiruPhoto[1];
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.PicFileName = picFileName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }



            }

            foreach (var data in dataArticle.fMissionList)
            {
                FinalMission finalmission = new FinalMission();
                finalmission.Title = data.auxiliary;
                finalmission.Main = data.auxiliarymain;
                finalmission.ArticleId = artId;
                finalmission.InitDateTime = DateTime.Now;
                db.FinalMissions.Add(finalmission);
            }

            Remark lastData = new Remark();
            lastData.ArticleId = artId;
            lastData.Main = dataArticle.final;
            lastData.InitTime = DateTime.Now;
            db.Remarks.Add(lastData);

            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "新增成功",
                artId = artId
            });

        }
        /// <summary>
        /// 訪客留言
        /// </summary>
        /// <param name="Main">留言內容</param>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddMessage( string Main, int artId)
        {
            var UserName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
        
            var UserID = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }
            else
            {
                Message message = new Message();
                message.MemberID = UserID;
                message.UserName = UserName;
                message.ArticleId = artId;
                message.Main = Main;
                message.InitDate = DateTime.Now;
                db.Messages.Add(message);
                db.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "已留言"
                });
            }
           
        }

        /// <summary>
        /// 取得此篇文章所有留言資料
        /// </summary>
        /// <param name="artId">文章Id</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllMessage(int artId ,int nowpage,int showcount)
        {
            //var data = db.Messages.Where(m => m.ArticleId == artId).Select(x=>new
            //{
            //    messageId=x.Id,
            //    messageUserName=x.Members.UserName,
            //    messageMember=x.Members.Name,
            //    messageMemberPic=x.Members.PicName+ "." + x.Members.FileName,
            //    messageMain =x.Main,
            //    messageInitDate=x.InitDate,
            //    reMessageData=x.R_Messages.Select(y=>new
            //    {
            //        reMessageId=y.Id,
            //        userName=y.Messages.Articles.UserName,
            //        author=y.Messages.Articles.AuthorName,
            //        authorPic=y.Messages.Articles.AuthorPic,
            //        reMessageMain=y.Main,
            //        reMessageInitDate=y.InitDate
            //    })

            //}).ToList();
            var data = db.Messages.Where(m => m.ArticleId == artId).Select(x => new
            {
                messageId = x.Id,
                messageUserName = x.Members.UserName,
                messageMember = x.Members.Name,
                messageMemberPic = x.Members.PicName + "." + x.Members.FileName,
                messageMain = x.Main,
                messageInitDate = x.InitDate,
                reMessageData = x.R_Messages.Join(db.Members,
                    a=>a.Messages.Articles.UserName,
                    b=>b.UserName,
                    (a,b)=>new
                    {
                        reMessageId = a.Id,
                        userName = b.UserName,
                        author = b.Name,
                        authorPic = b.PicName+"."+b.FileName,
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
                    })

            }).ToList();
            if (data != null)
            {
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
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });

            }

        }
        /// <summary>
        /// 取得留言資料(單筆)(含大頭貼)
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(OutPutMessage))]
        public IHttpActionResult Getmessage(int messageId)
        {
            var data = db.Messages.Where(x => x.Id == messageId).Select(y=>new
            {
                messageUserName = y.UserName,
                messageMember=y.Members.Name,
                messageMemberPic =y.Members.PicName+"."+y.Members.FileName,
                messageMain=y.Main,
                messageInitDate=y.InitDate
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
                data = data
            });
        }

        /// <summary>
        /// 回覆留言
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <param name="main">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddReMessage(int messageId, string main)
        {
            var data = db.Messages.FirstOrDefault(x => x.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });

            }
            int checkartId = data.ArticleId;
            var artData = db.Articles.FirstOrDefault(x => x.ID == checkartId).UserName;
            var memberUsername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);

            if (artData != memberUsername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            R_Message rMessage = new R_Message();
            rMessage.MessageId = messageId;
            rMessage.Main = main;
            rMessage.InitDate = DateTime.Now;
            db.R_Messages.Add(rMessage);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已回覆留言"
            });

        }

        /// <summary>
        /// 取得留言回覆的內容
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <returns></returns>
        [Route("api/Article/GetReMessage")]
        [ResponseType(typeof(OutPutReMessage))]
        [HttpGet]
        public IHttpActionResult GetReMessage(int messageId)
        {
            //var data = db.R_Messages.Where(m => m.MessageId == messageId).Select(x=>new
            //{
            //    reMessageId = x.Id,
            //    userName = x.Messages.Articles.UserName,
            //    author = x.Messages.Articles.AuthorName,
            //    authorPic = x.Messages.Articles.AuthorPic,
            //    reMessageMain = x.Main,
            //    reMessageInitDate = x.InitDate
            //}).ToList();
            var data = db.R_Messages.Where(m => m.MessageId == messageId).Join(
                db.Members,
                a=>a.Messages.Articles.UserName,
                b=>b.UserName,
                (a,b)=>new
                {
                    reMessageId = a.Id,
                    userName = b.UserName,
                    author = b.Name,
                    authorPic = b.PicName+"."+b.FileName,
                    reMessageMain = a.Main,
                    reMessageInitDate = a.InitDate
                }).Select(x => new
            {
                reMessageId = x.reMessageId,
                userName = x.userName,
                author = x.author,
                authorPic = x.authorPic,
                reMessageMain = x.reMessageMain,
                reMessageInitDate = x.reMessageInitDate
            }).ToList();
            
            if (data.Count > 0)
            {
                return Ok(new
                {
                    success = true,
                    data =data
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此留言"
                });
            }

        }
        /// <summary>
        /// 取得文章資料
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Article/intoArticle")]
        [ResponseType(typeof(Kirukiruoutput))]
        public IHttpActionResult GetArctile(int artId)
        {
            //var havdata = db.Articles.Where(m => m.ID == artId).Select(x=>new
            //{
            //    artId = x.ID,
            //    username = x.UserName,
            //    authorPic = x.AuthorPic,
            //    author = x.AuthorName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName +"."+ x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    articlecategoryId = x.ArticlecategoryId,
            //    artArtlog = x.Articlecategory.Name,
            //    isFree = x.IsFree,
            //    isPush = x.IsPush,
            //    lovecount=x.Lovecount,
            //    artInitDate=x.InitDate,
            //    fArrayList=x.Firstmissions.Select(y=>new
            //    {
            //        fId = y.Id,
            //        secPhoto = y.PicName + "." + y.PicFileName,
            //        mission = y.FirstItem,
            //    }),
            //    mArrayList=x.ArticleMains.Select(y=>new
            //    {
            //        mId = y.Id,
            //        thirdPhoto = y.PicName + "." + y.PicFileName,
            //        main = y.Main
            //    }),
            //    fMissionList=x.FinalMissions.Select(y=>new
            //    {
            //        fId = y.ID,
            //        auxiliary = y.Title,
            //        auxiliarymain = y.Main
            //    }),
            //    final = x.Remarks.FirstOrDefault(y=>y.ArticleId ==artId).Main
            //}).FirstOrDefault();

            var havdata = db.Articles.Join(
                db.Members,
                a => a.UserName,
                b => b.UserName,
                (a, b) => new
                {
                    artId = a.ID,
                    username = a.UserName,
                    authorPic = b.PicName + "." + b.FileName,
                    author = b.Name,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    articlecategoryId = a.ArticlecategoryId,
                    artArtlog = a.Articlecategory.Name,
                    isFree = a.IsFree,
                    isPush = a.IsPush,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate,
                    fArrayList = a.Firstmissions.Select(y => new
                    {
                        fId = y.Id,
                        secPhoto = y.PicName + "." + y.PicFileName,
                        mission = y.FirstItem,
                    }),
                    mArrayList = a.ArticleMains.Select(y => new
                    {
                        mId = y.Id,
                        thirdPhoto = y.PicName + "." + y.PicFileName,
                        main = y.Main
                    }),
                    fMissionList = a.FinalMissions.Select(y => new
                    {
                        fId = y.ID,
                        auxiliary = y.Title,
                        auxiliarymain = y.Main
                    }),
                    final = a.Remarks.FirstOrDefault(y => y.ArticleId == artId).Main
                }).Where(x => x.artId == artId).
                Select(x => new
            {
                artId = x.artId,
                username = x.username,
                authorPic = x.authorPic,
                author = x.author,
                title = x.title,
                firstPhoto = x.firstPhoto,
                introduction = x.introduction,
                articlecategoryId = x.articlecategoryId,
                artArtlog = x.artArtlog,
                isFree = x.isFree,
                isPush = x.isPush,
                lovecount = x.lovecount,
                artInitDate = x.artInitDate,
                fArrayList = x.fArrayList.Select(y => new
                {
                    fId = y.fId,
                    secPhoto = y.secPhoto,
                    mission = y.mission,
                }),
                mArrayList = x.mArrayList.Select(y => new
                {
                    mId = y.mId,
                    thirdPhoto = y.thirdPhoto,
                    main = y.main
                }),
                fMissionList = x.fMissionList.Select(y => new
                {
                    fId = y.fId,
                    auxiliary = y.auxiliary,
                    auxiliarymain = y.auxiliarymain
                }),
                final = x.final
            }).FirstOrDefault();
            
            if (havdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }
            
            return Ok(new
            {
                success = true,
                data =havdata
            });
        }

        /// <summary>
        /// 編輯文章(會把舊的圖片刪除後重新建立)
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[JwtAuthFilter]
        //[Route("EditArticle")]
        //public async Task<IHttpActionResult> EditArticle()
        //{
        //    var _ArtId = HttpContext.Current.Request.Form.GetValues("articleId"); ;
        //    int ArtId = Convert.ToInt32(_ArtId[0]);
        //    var _username = HttpContext.Current.Request.Form.GetValues("userName");
        //    string username = _username[0];

        //    var ArtData = db.Articles.FirstOrDefault(m => m.ID == ArtId);
        //    if (ArtData == null)
        //    {
        //        return Ok(new
        //        {
        //            success = false,
        //            message = "查無此文章"
        //        });
        //    }
        //    else
        //    {
        //        //try
        //        //{

        //        //}
        //        //catch (Exception e)
        //        //{
        //        //    return Ok(e.ToString());
        //        //    throw;
        //        //}

        //        string picpath = "~/Pic/";
        //        //刪除封面照片

        //        var otpicdata = db.Articles.FirstOrDefault(m => m.ID == ArtId);
        //        //刪除資料夾的圖片
        //        string picname = otpicdata.FirstPicName + "." + otpicdata.FirstPicFileName;
        //        string savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

        //        File.Delete(savpath);
        //        //db.FirstPics.Remove(otpicdata);
        //        //刪除前置步驟
        //        var OFpicary = db.Firstmissions.Where(m => m.ArticleId == ArtId).ToList();
        //        foreach (var pic in OFpicary)
        //        {
        //            int picid = pic.Id;
        //            var fdata = db.Firstmissions.FirstOrDefault(m => m.Id == picid);
        //            picname = fdata.PicName + "." + fdata.PicFileName;
        //            if (picname.Equals("."))
        //            {
        //                db.Firstmissions.Remove(fdata);
        //            }
        //            else
        //            {
        //                savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
        //                File.Delete(savpath);
        //                db.Firstmissions.Remove(fdata);
        //            }

        //        }
        //        //刪除切切
        //        var OMpicary = db.ArticleMains.Where(m => m.ArticleId == ArtId).ToList();
        //        foreach (var str in OMpicary)
        //        {
        //            int picid = str.Id;
        //            var mdata = db.ArticleMains.FirstOrDefault(m => m.Id == picid);
        //            picname = mdata.PicName + "." + mdata.PicFileName;
        //            if (picname.Equals("."))
        //            {
        //                db.ArticleMains.Remove(mdata);
        //            }
        //            else
        //            {
        //                savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
        //                File.Delete(savpath);
        //                db.ArticleMains.Remove(mdata);
        //            }

        //        }

        //        var oFinalMission = db.FinalMissions.Where(m => m.ArticleId == ArtId).ToList();
        //        foreach (var str in oFinalMission)
        //        {
        //            int oFinalId = str.ID;
        //            var odata = db.FinalMissions.FirstOrDefault(x => x.ID == oFinalId);
        //            db.FinalMissions.Remove(odata);
        //        }


        //        var ORemark = db.Remarks.Where(m => m.ArticleId == ArtId).ToList();
        //        foreach (var str in ORemark)
        //        {
        //            int remarkid = str.Id;
        //            var redata = db.Remarks.FirstOrDefault(m => m.Id == remarkid);
        //            db.Remarks.Remove(redata);
        //        }
        //        db.SaveChanges();

        //        if (!this.Request.Content.IsMimeMultipartContent())
        //        {
        //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //        }

        //        var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pic");
        //        var exists = Directory.Exists(root);
        //        if (!exists)
        //        {
        //            Directory.CreateDirectory("Pic");
        //        }

        //        var _title = HttpContext.Current.Request.Form.GetValues("title");
        //        string title = _title[0];
        //        var _Isfree = HttpContext.Current.Request.Form.GetValues("isFree");
        //        string Isfree = _Isfree[0];
        //        bool Is_Free = true;
        //        if (Isfree == "False")
        //        {
        //            Is_Free = false;
        //        }

        //        var _Introduction = HttpContext.Current.Request.Form.GetValues("introduction");
        //        string Introduction = _Introduction[0];
        //        var _ArticlecategoryId = HttpContext.Current.Request.Form.GetValues("articlecategoryId");
        //        string ArticlecategoryId = _ArticlecategoryId[0];
        //        var _IsPush = HttpContext.Current.Request.Form.GetValues("isPush");
        //        string IsPush = _IsPush[0];
        //        bool Is_Push = true;
        //        if (IsPush == "False")
        //        {
        //            Is_Push = false;
        //        }

        //        var _ArtMain = HttpContext.Current.Request.Form.GetValues("main");


        //        //下面是前置任務
        //        var _FirstMission = HttpContext.Current.Request.Form.GetValues("mission");

        //        int firstcount = 0;
        //        int artmaincount = 0;


        //        var provider = new MultipartMemoryStreamProvider();
        //        await this.Request.Content.ReadAsMultipartAsync(provider);


        //        foreach (var content in provider.Contents)
        //        {

        //            var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');

        //            if (KeyName.Contains("first"))
        //            {
        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                string[] fileary = filerealName.Split('.');
        //                var fileBytes = await content.ReadAsByteArrayAsync();

        //                var filefirst = DateTime.Now.ToFileTime() + username;
        //                var fileName = filefirst + "." + fileary[1];

        //                var outputPath = Path.Combine(root, fileName);
        //                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                {
        //                    await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                }

        //                var q = from p in db.Articles where p.ID == ArtId select p;
        //                foreach (var p in q)
        //                {
        //                    p.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
        //                    p.Title = title;
        //                    p.Introduction = Introduction;
        //                    p.IsFree = Is_Free;
        //                    p.IsPush = Is_Push;
        //                    p.FirstPicName = fileary[0];
        //                    p.FirstPicFileName = fileary[1];

        //                }

        //                db.SaveChanges();



        //            }
        //            else if (KeyName.Contains("sec"))
        //            {

        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                if (filerealName.Equals(""))
        //                {
        //                    Firstmission firstmission = new Firstmission();
        //                    firstmission.PicName = "";
        //                    firstmission.PicFileName = "";
        //                    firstmission.ArticleId = ArtId;
        //                    firstmission.FirstItem = _FirstMission[firstcount];
        //                    firstmission.InitDate = DateTime.Now;
        //                    db.Firstmissions.Add(firstmission);
        //                }
        //                else
        //                {
        //                    string[] fileary = filerealName.Split('.');
        //                    var fileBytes = await content.ReadAsByteArrayAsync();

        //                    var filefirst = DateTime.Now.ToFileTime() + username;
        //                    var fileName = filefirst + "." + fileary[1];

        //                    var outputPath = Path.Combine(root, fileName);
        //                    using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                    {
        //                        await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                    }

        //                    Firstmission firstmission = new Firstmission();
        //                    firstmission.PicName = filefirst;
        //                    firstmission.PicFileName = fileary[1];
        //                    firstmission.ArticleId = ArtId;
        //                    firstmission.FirstItem = _FirstMission[firstcount];
        //                    firstmission.InitDate = DateTime.Now;
        //                    db.Firstmissions.Add(firstmission);
        //                }

        //                //db.SaveChanges();
        //                firstcount++;
        //            }
        //            else if (KeyName.Contains("third"))
        //            {
        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                if (filerealName.Equals(""))
        //                {
        //                    ArticleMain articleMain = new ArticleMain();
        //                    articleMain.PicName = "";
        //                    articleMain.PicFileName = "";
        //                    articleMain.ArticleId = ArtId;
        //                    articleMain.Main = _ArtMain[artmaincount];
        //                    articleMain.InDateTime = DateTime.Now;
        //                    db.ArticleMains.Add(articleMain);
        //                    //db.SaveChanges();
        //                }
        //                else
        //                {
        //                    string[] fileary = filerealName.Split('.');
        //                    var fileBytes = await content.ReadAsByteArrayAsync();

        //                    var filefirst = DateTime.Now.ToFileTime() + username;
        //                    var fileName = filefirst + "." + fileary[1];

        //                    var outputPath = Path.Combine(root, fileName);
        //                    using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                    {
        //                        await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                    }

        //                    ArticleMain articleMain = new ArticleMain();
        //                    articleMain.PicName = filefirst;
        //                    articleMain.PicFileName = fileary[1];
        //                    articleMain.ArticleId = ArtId;
        //                    articleMain.Main = _ArtMain[artmaincount];
        //                    articleMain.InDateTime = DateTime.Now;
        //                    db.ArticleMains.Add(articleMain);

        //                    //db.SaveChanges();

        //                }
        //                artmaincount++;
        //            }
        //        }
        //        var _finMission = HttpContext.Current.Request.Form.GetValues("auxiliary");
        //        var _finMissionMain = HttpContext.Current.Request.Form.GetValues("auxiliarymain");
        //        int finMissionCount = 0;

        //        foreach (var content in provider.Contents)
        //        {
        //            var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');

        //            if (KeyName.Contains("first"))
        //            {
        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                string[] fileary = filerealName.Split('.');
        //                var fileBytes = await content.ReadAsByteArrayAsync();

        //                var filefirst = DateTime.Now.ToFileTime() + username;
        //                var fileName = filefirst + "." + fileary[1];

        //                var outputPath = Path.Combine(root, fileName);
        //                using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                {
        //                    await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                }
        //                Article article = new Article();

        //                article.UserName = username;
        //                article.Title = title;
        //                article.IsFree = Is_Free;
        //                article.Introduction = Introduction;
        //                article.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
        //                article.IsPush = Is_Push;
        //                article.FirstPicName = filefirst;
        //                article.FirstPicFileName = fileary[1];
        //                article.InitDate = DateTime.Now;
        //                db.Articles.Add(article);
        //                db.SaveChanges();
        //                var result = db.Articles.FirstOrDefault(m => m.FirstPicName == filefirst);
        //                ArtId = result.ID;


        //            }
        //            else if (KeyName.Contains("sec"))
        //            {

        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                if (filerealName.Equals(""))
        //                {
        //                    Firstmission firstmission = new Firstmission();
        //                    firstmission.PicName = "";
        //                    firstmission.PicFileName = "";
        //                    firstmission.ArticleId = ArtId;
        //                    firstmission.FirstItem = _FirstMission[firstcount];
        //                    firstmission.InitDate = DateTime.Now;
        //                    db.Firstmissions.Add(firstmission);
        //                }
        //                else
        //                {
        //                    string[] fileary = filerealName.Split('.');
        //                    var fileBytes = await content.ReadAsByteArrayAsync();

        //                    var filefirst = DateTime.Now.ToFileTime() + username;
        //                    var fileName = filefirst + "." + fileary[1];

        //                    var outputPath = Path.Combine(root, fileName);
        //                    using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                    {
        //                        await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                    }

        //                    Firstmission firstmission = new Firstmission();
        //                    firstmission.PicName = filefirst;
        //                    firstmission.PicFileName = fileary[1];
        //                    firstmission.ArticleId = ArtId;
        //                    firstmission.FirstItem = _FirstMission[firstcount];
        //                    firstmission.InitDate = DateTime.Now;
        //                    db.Firstmissions.Add(firstmission);
        //                }

        //                //db.SaveChanges();
        //                firstcount++;
        //            }
        //            else if (KeyName.Contains("third"))
        //            {
        //                var filerealName = content.Headers.ContentDisposition.FileName.Trim('\"');
        //                if (filerealName.Equals(""))
        //                {
        //                    ArticleMain articleMain = new ArticleMain();
        //                    articleMain.PicName = "";
        //                    articleMain.PicFileName = "";
        //                    articleMain.ArticleId = ArtId;
        //                    articleMain.Main = _ArtMain[artmaincount];
        //                    articleMain.InDateTime = DateTime.Now;
        //                    db.ArticleMains.Add(articleMain);
        //                    //db.SaveChanges();
        //                }
        //                else
        //                {
        //                    string[] fileary = filerealName.Split('.');
        //                    var fileBytes = await content.ReadAsByteArrayAsync();

        //                    var filefirst = DateTime.Now.ToFileTime() + username;
        //                    var fileName = filefirst + "." + fileary[1];

        //                    var outputPath = Path.Combine(root, fileName);
        //                    using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                    {
        //                        await output.WriteAsync(fileBytes, 0, fileBytes.Length);
        //                    }

        //                    ArticleMain articleMain = new ArticleMain();
        //                    articleMain.PicName = filefirst;
        //                    articleMain.PicFileName = fileary[1];
        //                    articleMain.ArticleId = ArtId;
        //                    articleMain.Main = _ArtMain[artmaincount];
        //                    articleMain.InDateTime = DateTime.Now;
        //                    db.ArticleMains.Add(articleMain);

        //                    //db.SaveChanges();

        //                }
        //                artmaincount++;
        //            }
        //            else
        //            {

        //            }

        //            //uploadResponse.Names.Add(fileName);
        //            //uploadResponse.FileNames.Add(outputPath);
        //            //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
        //        }
        //        if (_finMission.Length > 0)
        //        {
        //            foreach (var str in _finMission)
        //            {
        //                FinalMission finalMission = new FinalMission();
        //                finalMission.Title = str;
        //                finalMission.Main = _finMissionMain[finMissionCount];
        //                finalMission.InitDateTime = DateTime.Now;
        //                finalMission.ArticleId = ArtId;
        //                db.FinalMissions.Add(finalMission);
        //                finMissionCount++;
        //            }

        //        }


        //        var _Final = HttpContext.Current.Request.Form.GetValues("final");
        //        if (_Final != null)
        //        {
        //            foreach (var str in _Final)
        //            {
        //                Remark remark = new Remark();
        //                remark.Main = str;
        //                remark.InitTime = DateTime.Now;
        //                remark.ArticleId = ArtId;
        //                db.Remarks.Add(remark);

        //            }
        //            //db.SaveChanges();
        //        }
        //        db.SaveChanges();
        //        return Ok(new {
        //            success = true,
        //            message = "已編輯"
        //        });
        //    }


        //}
        [HttpPost]
        [JwtAuthFilter]
        [Route("EditArticle")]
        public IHttpActionResult EditArticle([FromBody] Editkirukiru editkirukiru)
        {
            var artId = editkirukiru.artId;
            var otpicdata = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (otpicdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此篇文章"
                });
            }

            var checkUserName = otpicdata.UserName;
            var jwtusername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUserName != jwtusername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }
           
            string picpath = "~/Pic/";
            //刪除封面照片

           
            //刪除資料夾的圖片
            string picname = otpicdata.FirstPicName + "." + otpicdata.FirstPicFileName;
            string savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

            //File.Delete(savpath);
            //db.FirstPics.Remove(otpicdata);
            //刪除前置步驟
            var OFpicary = db.Firstmissions.Where(m => m.ArticleId == artId).ToList();
            foreach (var pic in OFpicary)
            {
                int picid = pic.Id;
                var fdata = db.Firstmissions.FirstOrDefault(m => m.Id == picid);
                picname = fdata.PicName + "." + fdata.PicFileName;
                if (picname.Equals("."))
                {
                    db.Firstmissions.Remove(fdata);
                }
                else
                {
                    savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                    //File.Delete(savpath);
                    db.Firstmissions.Remove(fdata);
                }

            }
            //刪除切切
            var OMpicary = db.ArticleMains.Where(m => m.ArticleId == artId).ToList();
            foreach (var str in OMpicary)
            {
                int picid = str.Id;
                var mdata = db.ArticleMains.FirstOrDefault(m => m.Id == picid);
                picname = mdata.PicName + "." + mdata.PicFileName;
                if (picname.Equals("."))
                {
                    db.ArticleMains.Remove(mdata);
                }
                else
                {
                    savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                    //File.Delete(savpath);
                    db.ArticleMains.Remove(mdata);
                }

            }
            //刪除附屬任務
            var oFinalMission = db.FinalMissions.Where(m => m.ArticleId == artId).ToList();
            foreach (var str in oFinalMission)
            {
                int oFinalId = str.ID;
                var odata = db.FinalMissions.FirstOrDefault(x => x.ID == oFinalId);
                db.FinalMissions.Remove(odata);
            }
            //刪除備註
            var ORemark = db.Remarks.Where(m => m.ArticleId == artId).ToList();
            foreach (var str in ORemark)
            {
                int remarkid = str.Id;
                var redata = db.Remarks.FirstOrDefault(m => m.Id == remarkid);
                db.Remarks.Remove(redata);
            }
            db.SaveChanges();
            var username = editkirukiru.userName;
            if (string.IsNullOrWhiteSpace(editkirukiru.firstPhoto) || string.IsNullOrWhiteSpace(editkirukiru.title))
            {
                return Ok(new
                {
                    success = false,
                    message = "請上傳首圖和填寫標題"
                });
            }
            var artTitlePic = editkirukiru.firstPhoto.Split('.');
            string titlePicName = artTitlePic[0];
            string titleFileName = artTitlePic[1];

            var q = db.Articles.Where(p => p.ID == artId);
            //var q = from p in db.Articles where p.ID == artId select p;
            foreach (var p in q)
            {
                p.Title = editkirukiru.title;
                p.FirstPicName = titlePicName;
                p.FirstPicFileName = titleFileName;
                p.Introduction = editkirukiru.introduction;
                p.ArticlecategoryId = editkirukiru.articlecategoryId;
                p.IsFree = editkirukiru.isFree;
                p.IsPush = editkirukiru.isPush;
            }
           
            db.SaveChanges();
            

            var firstMission = editkirukiru.fArrayList.ToList();
            var kiruMain = editkirukiru.mArrayList.ToList();
            var finalMission = editkirukiru.fMissionList.ToList();
            var remark = editkirukiru.final;

            foreach (var data in firstMission)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.secPhoto))
                {
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.PicFileName = picFileName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);

                }
                else
                {
                    var PicName = data.secPhoto.Split('.');
                    picName = PicName[0];
                    picFileName = PicName[1];
                    Firstmission firstmission = new Firstmission();
                    firstmission.ArticleId = artId;
                    firstmission.PicName = picName;
                    firstmission.PicFileName = picFileName;
                    firstmission.FirstItem = data.mission;
                    firstmission.InitDate = DateTime.Now;
                    db.Firstmissions.Add(firstmission);
                }

            }

            foreach (var data in kiruMain)
            {
                string picName = "";
                string picFileName = "";
                if (string.IsNullOrWhiteSpace(data.thirdPhoto))
                {
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.PicFileName = picFileName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }
                else
                {
                    var kiruPhoto = data.thirdPhoto.Split('.');
                    picName = kiruPhoto[0];
                    picFileName = kiruPhoto[1];
                    ArticleMain articleMain = new ArticleMain();
                    articleMain.ArticleId = artId;
                    articleMain.PicName = picName;
                    articleMain.PicFileName = picFileName;
                    articleMain.Main = data.main;
                    articleMain.InDateTime = DateTime.Now;
                    db.ArticleMains.Add(articleMain);
                }



            }

            foreach (var data in finalMission)
            {
                FinalMission finalmission = new FinalMission();
                finalmission.Title = data.auxiliary;
                finalmission.Main = data.auxiliarymain;
                finalmission.ArticleId = artId;
                finalmission.InitDateTime = DateTime.Now;
                db.FinalMissions.Add(finalmission);
            }

            Remark lastData = new Remark();
            lastData.ArticleId = artId;
            lastData.Main = remark;
            lastData.InitTime = DateTime.Now;
            db.Remarks.Add(lastData);

            db.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "修改成功"
            });
        }
        /// <summary>
        /// 編輯用回傳文章資料
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        [Route("api/Article/Geteditarticle")]
        [ResponseType(typeof(KiruOutPutForEdit))]
        public IHttpActionResult GetEditArticle(int artId)
        {
            var jwtusername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var havdata = db.Articles.Where(m => m.ID == artId).Select(x=>new
            {
                artId = x.ID,
                title = x.Title,
                firstPhoto = x.FirstPicName+"."+x.FirstPicFileName,
                introduction = x.Introduction,
                articlecategoryId = x.ArticlecategoryId,
                artArtlog = x.Articlecategory.Name,
                fArrayList=x.Firstmissions.Select(y=>new
                {
                    fId = y.Id,
                    secPhoto = y.PicName + "." + y.PicFileName,
                    mission = y.FirstItem,
                }),
                mArrayList=x.ArticleMains.Select(y=>new
                {
                    mId = y.Id,
                    thirdPhoto = y.PicName + "." + y.PicFileName,
                    main = y.Main
                }),
                fMissionList=x.FinalMissions.Select(y=>new
                {
                    fId = y.ID,
                    auxiliary = y.Title,
                    auxiliarymain = y.Main
                }),
                isFree = x.IsFree,
                isPush = x.IsPush,
                artInitDate=x.InitDate,
                final = x.Remarks.FirstOrDefault(y=>y.ArticleId == artId).Main
            }).FirstOrDefault();
            if (havdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            var checkusername = db.Articles.FirstOrDefault(m => m.ID == artId).UserName;
           
            if (checkusername != jwtusername)
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
                data = havdata
            });

        }
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/Article/DeleteArticle")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteActile(int artId)
        {
            var ArtData = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (ArtData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            var checkUsername = ArtData.UserName;
            var jwtUsername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (checkUsername != jwtUsername)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }
            
            var ArtFirstMission = ArtData.Firstmissions.ToList();
            if (ArtFirstMission != null)
            {
                foreach (var str in ArtFirstMission)
                {
                    int FirstID = str.Id;
                    var FirstPic = db.Firstmissions.FirstOrDefault(m => m.Id == FirstID);
                    string picname = FirstPic.PicName + "." + FirstPic.PicFileName;

                    db.Firstmissions.Remove(FirstPic);
                    //db.SaveChanges();

                    string savepath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                    if (!picname.Equals("."))
                    {
                        //File.Delete(savepath);
                    }

                }
            }
            
            var ArtMain = ArtData.ArticleMains.ToList();
            if (ArtMain != null)
            {
                foreach (var str in ArtMain)
                {

                    int MainPicID = str.Id;

                    var MainPic = db.ArticleMains.FirstOrDefault(m => m.Id == MainPicID);
                    string picname = MainPic.PicName + "." + MainPic.PicFileName;
                    string savepath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

                    db.ArticleMains.Remove(MainPic);
                    //db.SaveChanges();
                    if (!picname.Equals("."))
                    {
                        //File.Delete(savepath);
                    }

                }
            }
            
            var ArtRemarke = ArtData.Remarks.ToList();
            if (ArtRemarke != null)
            {
                foreach (var str in ArtRemarke)
                {
                    int id = str.Id;
                    var result = db.Remarks.FirstOrDefault(m => m.Id == id);
                    db.Remarks.Remove(result);
                    //db.SaveChanges();
                }
            }
            

            var ArtMessage = ArtData.Messages.ToList();
            if (ArtMessage.Count > 0)
            {
                foreach (var message in ArtMessage)
                {
                    int id = message.Id;
                    var Rmessage = db.R_Messages.FirstOrDefault(m => m.MessageId == id);
                    var result = db.Messages.FirstOrDefault(m => m.Id == id);
                    db.R_Messages.Remove(Rmessage);

                    db.Messages.Remove(result);
                    //db.SaveChanges();
                }
            }

            var TitlePic = db.Articles.FirstOrDefault(m => m.ID == artId);
            string TitlePicName = TitlePic.FirstPicName;
            string TitlePicFileName = TitlePic.FirstPicFileName;
            db.Articles.Remove(ArtData);
            db.SaveChanges();
            string TitlePicfilestr = TitlePicName + "." + TitlePicFileName;
            string path = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{TitlePicfilestr}");
            //File.Delete(path);
            return Ok(new {
                success = true,
                message = "已刪除文章"
            });
            //try
            //{

            //    //string FilePath = WebConfigurationManager.ConnectionStrings["PicturePath"].ConnectionString;
            //   // string picpath = System.Web.HttpContext.Current.Server.MapPath($"~/testdatamodel/Pic/");

            //}
            //catch (Exception e)
            //{
            //    return Ok(e);
            //    throw;
            //}

        }
        /// <summary>
        /// 找到作者的所有切切
        /// </summary>
        /// <param name="ispush">是否發布(用來查詢是否在草稿</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">每頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserArticle( bool ispush, int nowpage,int showcount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            //var havedata = db.Articles.Where(m => m.UserName == username).Where(m=>m.IsPush == ispush).Select(x=>new
            //{
            //    artId=x.ID,
            //    author=x.AuthorName,
            //    authorPic=x.AuthorPic,
            //    username=x.UserName,
            //    title=x.Title,
            //    firstPhoto=x.FirstPicName+"."+x.FirstPicFileName,
            //    introduction=x.Introduction,
            //    artArtlog=x.Articlecategory.Name,
            //    articlecategoryId=x.ArticlecategoryId,
            //    isFree=x.IsFree,
            //    lovecount=x.Lovecount,
            //    messageCount=x.Messages.Count,
            //    artInitDate=x.InitDate

            //}).ToList();
            var memberData = db.Members.FirstOrDefault(m => m.UserName == username);
            if (memberData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            var havedata = db.Articles.Where(m => m.UserName == username &&  m.IsPush == ispush).Join(db.Members,
                a=>a.UserName,
                b=>b.UserName,
                (a,b)=>new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName+"."+b.FileName,
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


            int pagecount = havedata.Count;
            if (nowpage == 1)
            {
                var result = havedata.OrderByDescending(x => x.artInitDate).Take(showcount);
               
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

                var result = havedata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
            }
            
        }
        /// <summary>
        /// 按愛心
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult AddLoveArticle( int artId, bool putlove)
        {
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);
           
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
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
                success = true,
                lovecount= data.Lovecount
            });
        }
        /// <summary>
        /// 收藏切切文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult Collectarticle(int artId)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var datArticle = db.Articles.FirstOrDefault(x => x.ID == artId);

            if (datArticle == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == memberid);
            member.Articles.Add(datArticle);
            db.SaveChanges();
            return Ok(new {
                success = true,
                message = "已收藏"
            });

        }
        /// <summary>
        /// 取消收藏文章
        /// </summary>
        /// <param name="artId">收藏文章的ID</param>
        /// <returns></returns>
        [Route("api/Article/Deletecollect")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult Deletecollect(int artId)
        {
            var userid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Articles.FirstOrDefault(x => x.ID == artId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == userid);
            member.Articles.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success=true,
                message = "已取消收藏"
            });
        }
        /// <summary>
        /// 取得會員收藏的切切文章
        /// </summary>
        /// <param name="nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int nowpage, int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            //var data = db.Members.FirstOrDefault(m => m.ID == memberid).Articles.Select(x=>new
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
            //    messageCount = x.Messages.Count,
            //    artInitDate = x.InitDate
            //}).ToList();
            var data = db.Members.FirstOrDefault(m => m.ID == memberid).Articles.Join(db.Members,
                a => a.UserName,
                b => b.UserName,
                (a, b) => new
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
                var newArticles = data.OrderByDescending(x => x.artInitDate).Take(showcount);
               
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 依類別取得切切文章
        /// </summary>
        /// <param name="articlecategoryId">類別ID</param>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(KiruArtLogFourOutPut))]
        public IHttpActionResult GetArtlogArticle(int articlecategoryId,int nowpage, int showcount)
        {
            //var data = db.Articles.Where(x => x.ArticlecategoryId == articlecategoryId && x.IsPush == true)
            //    .Select(x=> new
            //    {
            //        artId=x.ID,
            //        username=x.UserName,
            //        author=x.AuthorName,
            //        authorPic=x.AuthorPic,
            //        title=x.Title,
            //        firstPhoto=x.FirstPicName+"."+x.FirstPicFileName,
            //        introduction=x.Introduction,
            //        artArtlog=x.Articlecategory.Name,
            //        articlecategoryId=x.ArticlecategoryId,
            //        isFree=x.IsFree,
            //        lovecount=x.Lovecount,
            //        artInitDate=x.InitDate,

            //    }).ToList();
            var data = db.Articles.Where(x => x.ArticlecategoryId == articlecategoryId && x.IsPush == true).Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
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
            })
                .Select(x => new
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

            if (data == null )
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
                var dataOutput = data.OrderByDescending(x => x.artInitDate).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = dataOutput
                });
            }
            else
            {
                int page = (nowpage - 1) * showcount;
                var dataOutput = data.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = dataOutput
                });
            }
           
        }
        /// <summary>
        /// 修改留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <param name="main">修改後內容</param>
        /// <returns></returns>
        [Route("api/Article/EditMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditMessage(int messageId,string main)
        {
            var messageData = db.Messages.FirstOrDefault(x => x.Id == messageId);
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
            //var q = db.Messages.FirstOrDefault(x => x.Id == messageId);
            //foreach (var str in q)
            //{
            //    str.Main = main;
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
        [Route("api/Article/EditReMessage")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditReMessage(int reMessageId, string main)
        {
            var reData = db.R_Messages.FirstOrDefault(x => x.Id == reMessageId);
            if (reData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此回覆"
                });
            }

            var checkMessageId = reData.MessageId;
            var checkArticleId = db.Messages.FirstOrDefault(x => x.Id == checkMessageId).ArticleId;
            var checkUserName = db.Articles.FirstOrDefault(x => x.ID == checkArticleId).UserName;
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
            //var q = db.R_Messages.FirstOrDefault(x => x.Id == reMessageId);
            //q.Main = main;
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
        /// <summary>
        /// 作者刪除留言
        /// </summary>
        /// <param name="messageId">留言ID</param>
        /// <returns></returns>
        [Route("api/Article/DeleteMessage")]
        [HttpDelete]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMessage(int messageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.Messages.FirstOrDefault(x => x.Id == messageId);
            var dataId = data.ArticleId;
            var artmember = db.Articles.FirstOrDefault(x => x.ID == dataId).UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限刪除留言"
                });
            }

            db.Messages.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
        /// <summary>
        /// 刪除切切作者回覆
        /// </summary>
        /// <param name="reMessageId">回覆ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Article/DeleteReMessage")]
        [JwtAuthFilter]
        public IHttpActionResult DeleteReMessage(int reMessageId)
        {
            var user = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.R_Messages.FirstOrDefault(x => x.Id == reMessageId);
            var messaageId = data.MessageId;
            var dataId = db.Messages.FirstOrDefault(x => x.Id == messaageId).ArticleId;
            var artmember = db.Articles.FirstOrDefault(x => x.ID == dataId).UserName;
            if (user != artmember)
            {
                return Ok(new
                {
                    success = false,
                    message = "你沒有權限"
                });
            }

            db.R_Messages.Remove(data);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "刪除留言"
            });

        }
    }
}
