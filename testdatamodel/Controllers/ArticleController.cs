using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using testdatamodel.Models;

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
                var _MemberID = HttpContext.Current.Request.Form.GetValues("MemberID");
                string MemberID = _MemberID[0];
                int UserID = Convert.ToInt32(MemberID);
                var _username = HttpContext.Current.Request.Form.GetValues("username");//找到formdata 文字檔 key值為 username的values值
                string username = _username[0];//value值
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
                List<int> artMainIdList = new List<int>();
                foreach (var ArtMain in _ArtMain)
                {
                    ArticleMainString articleMainString = new ArticleMainString();
                    articleMainString.Main = ArtMain;
                    articleMainString.InDateTime= DateTime.Now;
                    db.ArticleMainStrings.Add(articleMainString);
                    db.SaveChanges();
                    var result = db.ArticleMainStrings.FirstOrDefault(m => m.Main == ArtMain);
                    artMainIdList.Add(result.Id);
                }

                int[] ArtMainIDARY = artMainIdList.ToArray();
                int ArtMainStrId = 0;
                int ArtId = 0;

                //下面是前置任務
                var _FirstMission = HttpContext.Current.Request.Form.GetValues("Mission");
                List<int> FirstIdList = new List<int>();
                if (_FirstMission != null)
                {
                    
                    foreach (var FirstStr in _FirstMission)
                    {
                        FirstmissionString firstmissionString = new FirstmissionString();
                        firstmissionString.Main = FirstStr;
                        firstmissionString.InitDate = DateTime.Now;
                        db.FirstmissionStrings.Add(firstmissionString);
                        db.SaveChanges();
                        var result = db.FirstmissionStrings.FirstOrDefault(m => m.Main == FirstStr);
                        FirstIdList.Add(result.Id);
                    }

                }

                int[] Firstidary =FirstIdList.ToArray();
                int FirstId = 0;


                

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

                        FirstPic firstPic = new FirstPic();
                        firstPic.Name = fileary[0];
                        firstPic.FileName = fileary[1];
                        db.FirstPics.Add(firstPic);
                        db.SaveChanges();
                        string picname = fileary[0];
                        var Picresult = db.FirstPics.FirstOrDefault(m=>m.Name == picname);
                        int FirstPicId = Picresult.Id;

                        Article article = new Article();
                       
                        article.UserName = username;
                        article.Title = title;
                        article.IsFree = Is_Free;
                        article.Introduction = Introduction;
                        article.ArticlecategoryId = Convert.ToInt32(ArticlecategoryId);
                        article.IsPush = Is_Push;
                        article.FirstPicId = FirstPicId;
                        article.InitDate = DateTime.Now;
                        db.Articles.Add(article);
                        db.SaveChanges();
                        var result = db.Articles.FirstOrDefault(m => m.Title == title);
                        ArtId = result.ID;

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
                                db.SaveChanges();
                            }
                        }
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
                        firstmission.FirstmissionStringId = Firstidary[FirstId];
                        firstmission.InitDate = DateTime.Now;
                        db.Firstmissions.Add(firstmission);
                        db.SaveChanges();
                        FirstId++;

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
                        articleMain.InDateTime = DateTime.Now;
                        articleMain.ArticleMainStringId = ArtMainIDARY[ArtMainStrId];
                        articleMain.ArticleId = ArtId;
                        db.ArticleMains.Add(articleMain);
                        db.SaveChanges();
                        ArtMainStrId++;

                    }
                    else
                    {

                    }

                    //uploadResponse.Names.Add(fileName);
                    //uploadResponse.FileNames.Add(outputPath);
                    //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
                }

                return Ok();
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
            var data = db.Messages.Where(m => m.ArticleId ==artid).ToList();
            if (data.Count > 0)
            {
                ArrayList array = new ArrayList();
                foreach (var str in data)
                {
                    var result = new { str.Id, str.Main, str.InitDate };
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
            var result = new { message = "成功" };
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
            var data = db.R_Messages.Where(m => m.MessageId ==remsgid).ToList();
            if (data.Count > 0)
            {
                ArrayList arrayList = new ArrayList();
                foreach (var str in data)
                {
                    var result = new { str.Id, str.Main, str.InitDate };
                    arrayList.Add(result);
                }

                return Ok(arrayList);
            }
            else
            {
                return NotFound();
            }
           
        }
    }
}
