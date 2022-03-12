using System;
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
using testdatamodel.Models;
using testdatamodel.PutData;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 切切的文章
    /// </summary>
    public class ArticleController : ApiController
    {
        ProjectDb db = new ProjectDb();

        /// <summary>
        /// 添加切切文章
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IHttpActionResult> AddArticle()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pic");
            var exists = Directory.Exists(root);
            if (!exists)
            {
                Directory.CreateDirectory("Pic");
            }

            try
            {
                //var _MemberID = HttpContext.Current.Request.Form.GetValues("MemberID");
                //string MemberID = _MemberID[0];
                //int UserID = Convert.ToInt32(MemberID);
                var _username = HttpContext.Current.Request.Form.GetValues("username"); //找到formdata 文字檔 key值為 username的values值
                string username = _username[0]; //value值
                var _title = HttpContext.Current.Request.Form.GetValues("title");
                string title = _title[0];
                var _Isfree = HttpContext.Current.Request.Form.GetValues("Isfree");
                string Isfree = _Isfree[0];
                bool Is_Free = true;
                if (Isfree == "False")
                {
                    Is_Free = false;
                }

                var _Introduction = HttpContext.Current.Request.Form.GetValues("Introduction");
                string Introduction = _Introduction[0];
                var _ArticlecategoryId = HttpContext.Current.Request.Form.GetValues("ArticlecategoryId");
                string ArticlecategoryId = _ArticlecategoryId[0];
                var _IsPush = HttpContext.Current.Request.Form.GetValues("IsPush");
                string IsPush = _IsPush[0];
                bool Is_Push = true;
                if (IsPush == "False")
                {
                    Is_Push = false;
                }


                var _ArtMain = HttpContext.Current.Request.Form.GetValues("Main");
                int artmaincount = 0;



                //下面是前置任務
                var _FirstMission = HttpContext.Current.Request.Form.GetValues("Mission");
                int firstcount = 0;
                
                int ArtId = 0;
                
                var provider = new MultipartMemoryStreamProvider();
                await this.Request.Content.ReadAsMultipartAsync(provider);

                //var uploadResponse = new UploadResponse();
                foreach (var content in provider.Contents)
                {
                    var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');

                    if (KeyName.Contains("First"))
                    {
                        var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                        string[] fileary = fileName.Split('.');
                        var fileBytes = await content.ReadAsByteArrayAsync();

                        var outputPath = Path.Combine(root, fileName);
                        using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                        }


                        Article article = new Article();

                        article.UserName = username;
                        article.Title = title;
                        article.IsFree = Is_Free;
                        article.Introduction = Introduction;
                        article.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
                        article.IsPush = Is_Push;
                        article.FirstPicName = fileary[0];
                        article.FirstPicFileName = fileary[1];
                        article.InitDate = DateTime.Now;
                        db.Articles.Add(article);
                        db.SaveChanges();
                        var result = db.Articles.FirstOrDefault(m => m.Title == title);
                        ArtId = result.ID;

                    }
                    else if (KeyName.Contains("Sec"))
                    {
                        var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                        string[] fileary = fileName.Split('.');
                        var fileBytes = await content.ReadAsByteArrayAsync();

                        var outputPath = Path.Combine(root, fileName);
                        using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                        }

                        Firstmission firstmission = new Firstmission();
                        firstmission.PicName = fileary[0];
                        firstmission.PicFileName = fileary[1];
                        firstmission.ArticleId = ArtId;
                        firstmission.FirstItem = _FirstMission[firstcount];
                        firstmission.InitDate = DateTime.Now;
                        db.Firstmissions.Add(firstmission);
                        //db.SaveChanges();
                        firstcount++;
                    }
                    else if (KeyName.Contains("Third"))
                    {
                        var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                        string[] fileary = fileName.Split('.');
                        var fileBytes = await content.ReadAsByteArrayAsync();

                        var outputPath = Path.Combine(root, fileName);
                        using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                        }

                        ArticleMain articleMain = new ArticleMain();
                        articleMain.PicName = fileary[0];
                        articleMain.PicFileName = fileary[1];
                        articleMain.ArticleId = ArtId;
                        articleMain.Main = _ArtMain[artmaincount];
                        articleMain.InDateTime = DateTime.Now;
                        db.ArticleMains.Add(articleMain);
                        //db.SaveChanges();
                        artmaincount++;
                    }
                    else
                    {

                    }

                    //uploadResponse.Names.Add(fileName);
                    //uploadResponse.FileNames.Add(outputPath);
                    //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
                }
                
                
                var _Final = HttpContext.Current.Request.Form.GetValues("Final");
                if (_Final != null)
                {
                    foreach (var str in _Final)
                    {
                        Remark remark = new Remark();
                        remark.Main = str;
                        remark.InitTime = DateTime.Now;
                        remark.ArticleId = ArtId;
                        db.Remarks.Add(remark);

                    }
                    //db.SaveChanges();
                }
                db.SaveChanges();
                return Ok(new{status= "success" });
                //return this.Ok(uploadResponse);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message)
                });
            }
        }

        //[HttpGet]
        //public IHttpActionResult GetArticle(string username)
        //{
        //    var data = db.Articles.Where(m => m.UserName == username).ToList();
        //}
        /// <summary>
        /// 訪客留言
        /// </summary>
        /// <param name="UserName">訪客名稱</param>
        /// <param name="Main">留言內容</param>
        /// <param name="ArtID">文章ID</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMessage(string UserName, string Main, string ArtID)
        {
            Message message = new Message();
            message.UserName = UserName;
            message.ArticleId = Convert.ToInt32(ArtID);
            message.Main = Main;
            message.InitDate = DateTime.Now;
            db.Messages.Add(message);
            db.SaveChanges();
            var result = new {message = "成功"};
            return Ok(result);
        }

        /// <summary>
        /// 取得留言資料
        /// </summary>
        /// <param name="ArtId">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMessage(string ArtId)
        {
            int artid = Convert.ToInt32(ArtId);
            var data = db.Messages.Where(m => m.ArticleId == artid).ToList();
            if (data.Count > 0)
            {
                ArrayList array = new ArrayList();
                foreach (var str in data)
                {
                    var result = new {str.Id, str.Main, str.InitDate};
                    array.Add(result);
                }

                return Ok(array);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        /// 回覆留言
        /// </summary>
        /// <param name="Messageid">留言的ID</param>
        /// <param name="main">留言內容</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReMessage(string Messageid, string main)
        {
            R_Message rMessage = new R_Message();
            rMessage.MessageId = Convert.ToInt32(Messageid);
            rMessage.Main = main;
            rMessage.InitDate = DateTime.Now;
            db.R_Messages.Add(rMessage);
            db.SaveChanges();
            var result = new {message = "成功"};
            return Ok(result);
        }

        /// <summary>
        /// 取得留言回覆的內容
        /// </summary>
        /// <param name="ReMsgId">留言的ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReMessage(string ReMsgId)
        {
            int remsgid = Convert.ToInt32(ReMsgId);
            var data = db.R_Messages.Where(m => m.MessageId == remsgid).ToList();
            if (data.Count > 0)
            {
                ArrayList arrayList = new ArrayList();
                foreach (var str in data)
                {
                    var result = new {str.Id, str.Main, str.InitDate};
                    arrayList.Add(result);
                }

                return Ok(arrayList);
            }
            else
            {
                return NotFound();
            }

        }
        /// <summary>
        /// 取得文章資料
        /// </summary>
        /// <param name="articleid">文章ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArctile(string articleid)
        {
            int Art_ID = Convert.ToInt32(articleid);
            var havdata = db.Articles.FirstOrDefault(m => m.ID == Art_ID);
            if (havdata == null)
            {
                return Ok(new{status="沒有此文章ID"});
            }
            
            var Art_Title = havdata.Title.ToString();
            var Art_TitlePic = havdata.FirstPicName.ToString() + "." +
                               havdata.FirstPicFileName.ToString();
            var Art_Info = havdata.Introduction;
            var Art_Artlog = havdata.Articlecategory.Name.ToString();

            var Art_FirstMissionData = havdata.Firstmissions.ToList();
            ArrayList fArrayList = new ArrayList();
            foreach (var str in Art_FirstMissionData)
            {
                var Fdata = new
                {
                    FID = str.Id,
                    FpicName = str.PicName + "." + str.PicFileName,
                    Fstr = str.FirstItem,
                };
                fArrayList.Add(Fdata);
            }

            var Art_Isfree = havdata.IsFree.ToString();
            var Art_IsPush = havdata.IsPush.ToString();
            var Art_MainData = havdata.ArticleMains.ToList();
            var Art_message = havdata.Messages.ToList();
            
            ArrayList mArrayList = new ArrayList();
            foreach (var str in Art_MainData)
            {
                var Mdata = new
                {
                    MId = str.Id,
                    MpicName = str.PicName + "." + str.PicFileName,
                    Mstr = str.Main
                };
                mArrayList.Add(Mdata);
            }
            ArrayList messageArrayList = new ArrayList();
            ArrayList remessageArrayList = new ArrayList();
            foreach (var str in Art_message)
            {
                
                var mdata = new
                {
                    messageid = str.Id,
                    messagemain = str.Main,
                    messageinitdate=str.InitDate,
                };
                messageArrayList.Add(mdata);
                var rmessagedata = str.R_Messages.ToList();
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
            var result = new
            {
                Art_ID,
                Art_Title,
                Art_TitlePic,
                Art_Info,
                Art_Artlog,
                fArrayList,
                mArrayList,
                Art_Isfree,
                Art_IsPush,
                messageArrayList,
                remessageArrayList,
               
            };
            return Ok(result);
        }
        /// <summary>
        /// 編輯文章(會把舊的圖片刪除後重新建立)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EditArticle")]
        public async Task<IHttpActionResult> EditArticle()
        {
            var _ArtId = HttpContext.Current.Request.Form.GetValues("ArticleID"); ;
            int ArtId = Convert.ToInt32(_ArtId[0]);
            var ArtData = db.Articles.FirstOrDefault(m => m.ID == ArtId);
            if (ArtData == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string picpath = "~/Pic/";
                    //刪除封面照片

                    var otpicdata = db.Articles.FirstOrDefault(m => m.ID == ArtId);
                    //刪除資料夾的圖片
                    string picname = otpicdata.FirstPicName + "." + otpicdata.FirstPicFileName;
                    string savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");

                    File.Delete(savpath);
                    //db.FirstPics.Remove(otpicdata);
                    //刪除前置步驟
                    var OFpicary = db.Firstmissions.Where(m => m.ArticleId == ArtId).ToList();
                    foreach (var pic in OFpicary)
                    {
                        int picid = pic.Id;
                        var fdata = db.Firstmissions.FirstOrDefault(m => m.Id == picid);
                        picname = fdata.PicName + "." + fdata.PicFileName;
                        savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                        File.Delete(savpath);
                        db.Firstmissions.Remove(fdata);
                    }
                    //刪除切切
                    var OMpicary = db.ArticleMains.Where(m => m.ArticleId == ArtId).ToList();
                    foreach (var str in OMpicary)
                    {
                        int picid = str.Id;
                        var mdata = db.ArticleMains.FirstOrDefault(m => m.Id == picid);
                        picname = mdata.PicName + "." + mdata.PicFileName;
                        savpath = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{picname}");
                        File.Delete(savpath);
                        db.ArticleMains.Remove(mdata);
                    }

                    var ORemark = db.Remarks.Where(m => m.ArticleId == ArtId).ToList();
                    foreach (var str in ORemark)
                    {
                        int remarkid = str.Id;
                        var redata = db.Remarks.FirstOrDefault(m => m.Id == remarkid);
                        db.Remarks.Remove(redata);
                    }
                    db.SaveChanges();

                    if (!this.Request.Content.IsMimeMultipartContent())
                    {
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                    }

                    var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pic");
                    var exists = Directory.Exists(root);
                    if (!exists)
                    {
                        Directory.CreateDirectory("Pic");
                    }

                    var _title = HttpContext.Current.Request.Form.GetValues("title");
                    string title = _title[0];
                    var _Isfree = HttpContext.Current.Request.Form.GetValues("Isfree");
                    string Isfree = _Isfree[0];
                    bool Is_Free = true;
                    if (Isfree == "False")
                    {
                        Is_Free = false;
                    }

                    var _Introduction = HttpContext.Current.Request.Form.GetValues("Introduction");
                    string Introduction = _Introduction[0];
                    var _ArticlecategoryId = HttpContext.Current.Request.Form.GetValues("ArticlecategoryId");
                    string ArticlecategoryId = _ArticlecategoryId[0];
                    var _IsPush = HttpContext.Current.Request.Form.GetValues("IsPush");
                    string IsPush = _IsPush[0];
                    bool Is_Push = true;
                    if (IsPush == "False")
                    {
                        Is_Push = false;
                    }

                    var _ArtMain = HttpContext.Current.Request.Form.GetValues("Main");
                    

                    //下面是前置任務
                    var _FirstMission = HttpContext.Current.Request.Form.GetValues("Mission");

                    int firstcount = 0;
                    int artmaincount = 0;


                    var provider = new MultipartMemoryStreamProvider();
                    await this.Request.Content.ReadAsMultipartAsync(provider);


                    foreach (var content in provider.Contents)
                    {

                        var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');

                        if (KeyName.Contains("First"))
                        {
                            var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                            string[] fileary = fileName.Split('.');
                            var fileBytes = await content.ReadAsByteArrayAsync();

                            var outputPath = Path.Combine(root, fileName);
                            using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                            }

                            var q = from p in db.Articles where p.ID == ArtId select p;
                            foreach (var p in q)
                            {
                                p.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
                                p.Title = title;
                                p.Introduction = Introduction;
                                p.IsFree = Is_Free;
                                p.IsPush = Is_Push;
                                p.FirstPicName = fileary[0];
                                p.FirstPicFileName = fileary[1];

                            }

                            db.SaveChanges();


                            
                        }
                        else if (KeyName.Contains("Sec"))
                        {
                            var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                            string[] fileary = fileName.Split('.');
                            var fileBytes = await content.ReadAsByteArrayAsync();

                            var outputPath = Path.Combine(root, fileName);
                            using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                            }

                            Firstmission firstmission = new Firstmission();
                            firstmission.PicName = fileary[0];
                            firstmission.PicFileName = fileary[1];
                            firstmission.ArticleId = ArtId;
                            firstmission.FirstItem = _FirstMission[firstcount];
                            firstmission.InitDate = DateTime.Now;
                            db.Firstmissions.Add(firstmission);
                            //db.SaveChanges();
                            firstcount++;

                        }
                        else if (KeyName.Contains("Third"))
                        {
                            var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                            string[] fileary = fileName.Split('.');
                            var fileBytes = await content.ReadAsByteArrayAsync();

                            var outputPath = Path.Combine(root, fileName);
                            using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                            }


                            ArticleMain articleMain = new ArticleMain();
                            articleMain.PicName = fileary[0];
                            articleMain.PicFileName = fileary[1];
                            articleMain.ArticleId = ArtId;
                            articleMain.Main = _ArtMain[artmaincount];
                            articleMain.InDateTime = DateTime.Now;
                            db.ArticleMains.Add(articleMain);
                            //db.SaveChanges();
                            artmaincount++;
                        }
                    }

                    var _Final = HttpContext.Current.Request.Form.GetValues("Final");
                    if (_Final != null)
                    {
                        foreach (var str in _Final)
                        {
                            Remark remark = new Remark();
                            remark.Main = str;
                            remark.InitTime = DateTime.Now;
                            remark.ArticleId = ArtId;
                            db.Remarks.Add(remark);

                        }
                        //db.SaveChanges();
                    }
                    db.SaveChanges();
                    return Ok(new { status = "success" });
                }
                catch (Exception e)
                {
                    return Ok(e.ToString());
                    throw;
                }
            }


        }

        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteActile(string artid)
        {
            int artId = Convert.ToInt32(artid);
            var ArtData = db.Articles.FirstOrDefault(m => m.ID == artId);
            if (ArtData == null)
            {
                return NotFound();
            }

            try
            {

                //string FilePath = WebConfigurationManager.ConnectionStrings["PicturePath"].ConnectionString;
               // string picpath = System.Web.HttpContext.Current.Server.MapPath($"~/testdatamodel/Pic/");
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
                var ArtMain =ArtData.ArticleMains.ToList();
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
                
                var TitlePic = db.Articles.FirstOrDefault(m => m.ID==artId);
                string TitlePicName = TitlePic.FirstPicName;
                string TitlePicFileName = TitlePic.FirstPicFileName;
                db.Articles.Remove(ArtData);
                db.SaveChanges();
                string TitlePicfilestr = TitlePicName + "." + TitlePicFileName;
                string path = System.Web.HttpContext.Current.Server.MapPath($"~/Pic/{TitlePicfilestr}");
                File.Delete(path);
                return Ok(new {status = "success" });
            }
            catch (Exception e)
            {
                return Ok(e);
                throw;
            }

        }
        /// <summary>
        /// 找到作者的所有切切
        /// </summary>
        /// <param name="username">作者的會員帳號</param>
        /// <param name="ispush">是否發布</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserArticle(string username, bool ispush)
        {
            var havedata = db.Articles.FirstOrDefault(m => m.UserName == username);
            if (havedata == null)
            {
                return NotFound();
            }
            var data = from q in db.Articles
                where (q.UserName == username & q.IsPush == ispush)
                select q;
            ArrayList arrayList = new ArrayList();
            foreach (var art in data.ToList())
            {
                var result = new
                {
                    art.ID,
                    art.Title,
                    art.Introduction,
                    picture = art.FirstPicName + "." + art.FirstPicFileName,
                    art.Articlecategory.Name,
                    art.IsFree,
                    art.Lovecount,
                    art.InitDate
                };
                arrayList.Add(result);
            }

            return Ok(arrayList);
        }
        /// <summary>
        /// 按愛心
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <param name="putlove">是否按愛心</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult AddLoveArticle( int artid,bool putlove)
        {
            var data = from q in db.Articles
                where (q.ID == artid)
                select q;
            if (data == null)
            {
                return NotFound();
            }

            int lovecount = db.Articles.FirstOrDefault(m => m.ID == artid).Lovecount;
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

            return Ok(new {status = "success" });
        }
        /// <summary>
        /// 收藏切切文章
        /// </summary>
        /// <param name="artid">文章ID</param>
        /// <param name="memberid">收藏者的ID</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Collectarticle(int artid, int memberid)
        {
            var datArticle = db.Articles.FirstOrDefault(x => x.ID == artid);

            if (datArticle == null)
            {
                return NotFound();
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == memberid);
            member.Articles.Add(datArticle);
            db.SaveChanges();
            return Ok(new {status = "success" });

        }
        /// <summary>
        /// 取消收藏文章
        /// </summary>
        /// <param name="userid">會員ID</param>
        /// <param name="articleid">收藏文章的ID</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Deletecollect(int userid, int articleid)
        {
            var data = db.Articles.FirstOrDefault(x => x.ID == articleid);
            if (data == null)
            {
                return NotFound();
            }

            Member member = db.Members.FirstOrDefault(x => x.ID == userid);
            member.Articles.Remove(data);
            db.SaveChanges();
            return Ok(new { status = "success" });
        }
        /// <summary>
        /// 取得會員收藏的切切文章
        /// </summary>
        /// <param name="memberid">會員ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllcollectart(int memberid)
        {
            var data = db.Members.FirstOrDefault(x => x.ID == memberid);
            var art = data.Articles.ToList();
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
