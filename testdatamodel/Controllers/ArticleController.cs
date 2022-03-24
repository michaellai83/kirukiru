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
        //[JwtAuthFilter]
        public IHttpActionResult AddArticle([FromBody] DataArticle dataArticle)
        {
            /// var jObject = JsonConvert.SerializeObject(dataArticleInput);
            //var dataArticle = JsonConvert.DeserializeObject<DataArticle>(jObject);
            var username = dataArticle.memberUserName;
            var arttitle = dataArticle.title;
            if (dataArticle.firstPhoto.Equals("") || dataArticle.title.Equals("") || dataArticle.articlecategoryId == 0)
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
            var artinfo = dataArticle.introduction;

            var artlogid = dataArticle.articlecategoryId;
            var artisFree = dataArticle.isFree;
            //bool isfree = true;
            //if (artisFree.Equals("False"))
            //{
            //    isfree = false;
            //}
            var artisPush = dataArticle.isPush;
            //bool ispush = true;
            //if (artisPush.Equals("False"))
            //{
            //    ispush = false;
            //}
            Article article = new Article();
            article.UserName = username;
            article.Title = arttitle;
            article.FirstPicName = titlePicName;
            article.FirstPicFileName = titleFileName;
            article.Introduction = artinfo;
            article.ArticlecategoryId = artlogid;
            article.InitDate = DateTime.Now;
            article.IsFree = artisFree;
            article.IsPush = artisPush;
            article.Lovecount = 0;
            db.Articles.Add(article);
            db.SaveChanges();
            var artId = db.Articles.FirstOrDefault(x => x.FirstPicName == titlePicName).ID;


            var firstMission = dataArticle.fArrayList;
            var kiruMain = dataArticle.mArrayList;
            var finalMission = dataArticle.fMissionList;
            var remark = dataArticle.final;

            foreach (var data in firstMission)
            {
                string picName = "";
                string picFileName = "";
                if (data.secPhoto.Equals(""))
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

            foreach (var data in kiruMain)
            {
                string picName = "";
                string picFileName = "";
                if (data.thirdPhoto.Equals(""))
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
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllMessage(int artId)
        {
            var data = db.Messages.Where(m => m.ArticleId == artId).ToList();
            if (data.Count > 0)
            {
                ArrayList array = new ArrayList();
                foreach (var str in data)
                {
                    var result = new
                    {
                        messageId = str.Id,
                        messageMember = str.UserName,
                        messageMain = str.Main,
                        messageInitDate = str.InitDate
                    };
                    array.Add(result);
                }

                return Ok(new
                {
                    success = true,
                    data =array
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
        /// 取得留言資料(單筆)
        /// </summary>
        /// <param name="messageId">留言的ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Getmessage(int messageId)
        {
            var data = db.Messages.FirstOrDefault(x => x.Id == messageId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此留言"
                });
            }

            var result = new
            {
                messageMember=data.UserName,
                messageMain = data.Main,
                messageInitDate = data.InitDate
            };
            return Ok(new
            {
                success = true,
                data = result
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
        [HttpGet]
        public IHttpActionResult GetReMessage(int messageId)
        {
            var data = db.R_Messages.Where(m => m.MessageId == messageId).ToList();
            if (data.Count > 0)
            {
                ArrayList arrayList = new ArrayList();
                foreach (var str in data)
                {
                    var result = new {
                        reMessageId = str.Id,
                        reMessageMain = str.Main,
                        reMessageInitDate = str.InitDate};
                    arrayList.Add(result);
                }

                return Ok(new
                {
                    success = true,
                    data =arrayList
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
            var havdata = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (havdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }
            
            var Art_Title = havdata.Title.ToString();
            var Art_TitlePic = havdata.FirstPicName.ToString() + "." +
                               havdata.FirstPicFileName.ToString();
            var Art_Info = havdata.Introduction;
            var Art_Artlog = havdata.Articlecategory.Name.ToString();
            var artlogid = havdata.Articlecategory.Id;
            var lovecount = havdata.Lovecount;
            var Art_FirstMissionData = havdata.Firstmissions.ToList();
            ArrayList fArrayList = new ArrayList();
            foreach (var str in Art_FirstMissionData)
            {
                var Fdata = new
                {
                    fId = str.Id,
                    secPhoto = str.PicName + "." + str.PicFileName,
                    mission = str.FirstItem,
                };
                fArrayList.Add(Fdata);
            }

            var Art_Isfree = havdata.IsFree.ToString();
            var Art_IsPush = havdata.IsPush.ToString();
            var Art_MainData = havdata.ArticleMains.ToList();
            var Art_message = havdata.Messages.ToList();
            var ArtInitDate = havdata.InitDate.ToString();
            var artRemark = havdata.Remarks.ToList();

            var finalMission = havdata.FinalMissions.ToList();
            ArrayList fMissionList = new ArrayList();
            foreach (var str in finalMission)
            {
                var fdata = new
                {
                    fId = str.ID,
                    auxiliary = str.Title,
                    auxiliarymain = str.Main
                };
                fMissionList.Add(fdata);
            }
            
            
            ArrayList mArrayList = new ArrayList();
            foreach (var str in Art_MainData)
            {
                var Mdata = new
                {
                    mId = str.Id,
                    thirdPhoto = str.PicName + "." + str.PicFileName,
                    main = str.Main
                };
                mArrayList.Add(Mdata);
            }
            ArrayList messageArrayList = new ArrayList();
            ArrayList remessageArrayList = new ArrayList();
            foreach (var str in Art_message)
            {
                var rmessagedata = str.R_Messages.ToList();
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
                var mdata = new
                {
                    messageId = str.Id,
                    messageMember = str.UserName,
                    messageMain = str.Main,
                    messageInitDate =str.InitDate,
                    reMessageData= remessageArrayList
                };
                messageArrayList.Add(mdata);
                
            }

            string artRemarkStr = "";
            foreach (var str in artRemark)
            {
                artRemarkStr = str.Main;
            }
            
            var result = new
            {
                artId= artId,
                title = Art_Title,
                firstPhoto = Art_TitlePic,
                introduction = Art_Info,
                articlecategoryId=artlogid,
                artArtlog =Art_Artlog,
                fArrayList,
                mArrayList,
                fMissionList,
                isFree = Art_Isfree,
                isPush = Art_IsPush,
                lovecount,
                ArtInitDate,
                messageArrayList,
                final= artRemarkStr
                //reMessageArrayList=remessageArrayList,

            };
            return Ok(new
            {
                success = true,
                data =result
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

           
            string picpath = "~/Pic/";
            //刪除封面照片

           
            //刪除資料夾的圖片
            string picname = otpicdata.FirstPicName + "." + otpicdata.FirstPicFileName;
            string savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

            File.Delete(savpath);
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
                    File.Delete(savpath);
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
                    File.Delete(savpath);
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
            var arttitle = editkirukiru.title;
            if (editkirukiru.firstPhoto.Equals("") && editkirukiru.title.Equals(""))
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
            var artinfo = editkirukiru.introduction;

            var artlogid = editkirukiru.articlecategoryId;
            var artisFree = editkirukiru.isFree;
            //bool isfree = true;
            //if (artisFree.Equals("False"))
            //{
            //    isfree = false;
            //}
            var artisPush = editkirukiru.isPush;
            //bool ispush = true;
            //if (artisPush.Equals("False"))
            //{
            //    ispush = false;
            //}
            var q = from p in db.Articles where p.ID == artId select p;
            foreach (var p in q)
            {
                p.Title= arttitle;
                p.UserName = username;
                p.Title = arttitle;
                p.FirstPicName = titlePicName;
                p.FirstPicFileName = titleFileName;
                p.Introduction = artinfo;
                p.ArticlecategoryId = artlogid;
                p.InitDate = DateTime.Now;
                p.IsFree = artisFree;
                p.IsPush = artisPush;
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
                if (data.secPhoto.Equals(""))
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
                if (data.thirdPhoto.Equals(""))
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
            var havdata = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (havdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此文章"
                });
            }

            var Art_Title = havdata.Title.ToString();
            var Art_TitlePic = havdata.FirstPicName.ToString() + "." +
                               havdata.FirstPicFileName.ToString();
            var Art_Info = havdata.Introduction;
            var Art_Artlog = havdata.Articlecategory.Name.ToString();
            var artlogid = havdata.Articlecategory.Id;

            var Art_FirstMissionData = havdata.Firstmissions.ToList();
            ArrayList fArrayList = new ArrayList();
            foreach (var str in Art_FirstMissionData)
            {
                var Fdata = new
                {
                    fId = str.Id,
                    secPhoto = str.PicName + "." + str.PicFileName,
                    mission = str.FirstItem,
                };
                fArrayList.Add(Fdata);
            }

            var Art_Isfree = havdata.IsFree;
            var Art_IsPush = havdata.IsPush;
            var Art_MainData = havdata.ArticleMains.ToList();
            var Art_message = havdata.Messages.ToList();
            var ArtInitDate = havdata.InitDate.ToString();
            var artRemark = havdata.Remarks.ToList();

            var finalMission = havdata.FinalMissions.ToList();
            ArrayList fMissionList = new ArrayList();
            foreach (var str in finalMission)
            {
                var fdata = new
                {
                    fId = str.ID,
                    auxiliary = str.Title,
                    auxiliarymain = str.Main
                };
                fMissionList.Add(fdata);
            }


            ArrayList mArrayList = new ArrayList();
            foreach (var str in Art_MainData)
            {
                var Mdata = new
                {
                    mId = str.Id,
                    thirdPhoto = str.PicName + "." + str.PicFileName,
                    main = str.Main
                };
                mArrayList.Add(Mdata);
            }

            string artRemarkStr = "";
            foreach (var str in artRemark)
            {
                artRemarkStr = str.Main;
            }

            var result = new
            {
                artId = artId,
                title = Art_Title,
                firstPhoto = Art_TitlePic,
                introduction = Art_Info,
                articlecategoryId = artlogid,
                artArtlog = Art_Artlog,
                fArrayList,
                mArrayList,
                fMissionList,
                isFree = Art_Isfree,
                isPush = Art_IsPush,
                ArtInitDate,
                final = artRemarkStr
            };
            return Ok(new
            {
                success = true,
                data = result
            });

        }
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="artId">文章ID</param>
        /// <returns></returns>
        [Route("api/Article/DeleteActile")]
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
            foreach (var str in ArtFirstMission)
            {
                int FirstID = str.Id;
                var FirstPic = db.Firstmissions.FirstOrDefault(m => m.Id == FirstID);
                string picname = FirstPic.PicName + "." + FirstPic.PicFileName;

                db.Firstmissions.Remove(FirstPic);
                //db.SaveChanges();

                string savepath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                File.Delete(savepath);
            }
            var ArtMain = ArtData.ArticleMains.ToList();
            foreach (var str in ArtMain)
            {

                int MainPicID = str.Id;

                var MainPic = db.ArticleMains.FirstOrDefault(m => m.Id == MainPicID);
                string picname = MainPic.PicName + "." + MainPic.PicFileName;
                string savepath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

                db.ArticleMains.Remove(MainPic);
                //db.SaveChanges();
                File.Delete(savepath);
            }

            var ArtRemarke = ArtData.Remarks.ToList();
            foreach (var str in ArtRemarke)
            {
                int id = str.Id;
                var result = db.Remarks.FirstOrDefault(m => m.Id == id);
                db.Remarks.Remove(result);
                //db.SaveChanges();
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
            File.Delete(path);
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
        /// <param name="nowPage">現在頁數(預設1)</param>
        /// <param name="showCount">每頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetUserArticle( bool ispush, int nowPage,int showCount)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var havedata = db.Articles.FirstOrDefault(m => m.UserName == username);
            if (havedata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有文章"
                });

            }
            var data = from q in db.Articles
                where (q.UserName == username & q.IsPush == ispush)
                select q;
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

            int pagecount = arrayList.Count;
            if (nowPage == 1)
            {
                var result = arrayList.OrderByDescending(x => x.InitDateTime).Take(showCount);
               
                return Ok(new
                {
                    success = true,
                    total = pagecount,
                    data = result
                });
            }
            else
            {
                int page = (nowPage - 1) * showCount;
                //排序依照日期

                var result = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showCount);
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
            var data = from q in db.Articles
                where (q.ID == artId)
                select q;
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此文章"
                });
            }

            int lovecount = db.Articles.FirstOrDefault(m => m.ID == artId).Lovecount;
            if (putlove == true)
            {
                lovecount ++;
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
                success = true
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
        /// <param name="Nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetAllcollectart(int Nowpage, int showcount)
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Members.FirstOrDefault(x => x.ID == memberid);
            var art = data.Articles.ToList();
            //ArrayList artlList = new ArrayList();
            //foreach (var str in art)
            //{
            //    var result = new
            //    {
            //        str.ID,
            //        str.Articlecategory.Name,
            //        str.UserName,
            //        str.Title,
            //    };
            //    artlList.Add(result);
            //}
            List<NewArticle> arrayList = new List<NewArticle>();
            foreach (var content in art)
            {

                NewArticle newartary = new NewArticle();
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
                var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
                ArrayList result = new ArrayList();
                foreach (var str in newArticles)
                {
                    var resultdata = new
                    {
                        str.ArticleID,
                        str.UserName,
                        str.Title,
                        str.Articlecategory,
                        str.Lovecount,
                        str.InitDateTime
                    };
                    result.Add(resultdata);
                }
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }
            else
            {
                var page = (Nowpage - 1) * showcount;
                var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
                ArrayList result = new ArrayList();
                foreach (var str in newArticles)
                {
                    var resultdata = new
                    {
                        str.ArticleID,
                        str.UserName,
                        str.Title,
                        str.Articlecategory,
                        str.Lovecount,
                        str.InitDateTime
                    };
                    result.Add(resultdata);
                }
                return Ok(new
                {
                    success = true,
                    total = total,
                    data = result
                });
            }

        }
        /// <summary>
        /// 依類別取得四筆切切文章
        /// </summary>
        /// <param name="articlecategoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(KiruArtLogFourOutPut))]
        public IHttpActionResult GetArtlogArticle(int articlecategoryId)
        {
            var data = db.Articles.Where(x => x.ArticlecategoryId == articlecategoryId).Where(x=>x.IsPush == true)
                .OrderByDescending(x => x.InitDate).Take(4);
            var kiruData = data.ToList();
            if (kiruData == null )
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有相關文章"
                });
            }

            
            List<NewArticle> arrayList = new List<NewArticle>();
            foreach (var content in kiruData)
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
            return Ok(new
            {
                success = true,
                data = arrayList
            });
        }
    }
}
