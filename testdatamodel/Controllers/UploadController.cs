using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using testdatamodel.JWT;
using testdatamodel.Models;

namespace testdatamodel.Controllers
{
     
    public class UploadController : ApiController
    {
        ProjectDb db = new ProjectDb();
        /// <summary>
        /// 新頭像上傳方法，通過表單的形式上傳
        /// </summary>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        [JwtAuthFilter]
        public async Task<IHttpActionResult> Upload()
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
                
                var _username = HttpContext.Current.Request.Form.GetValues("username");//找到formdata 文字檔 key值為 username的values值
                string username = _username[0];//value值
                var provider = new MultipartMemoryStreamProvider();
                await this.Request.Content.ReadAsMultipartAsync(provider);

                //var uploadResponse = new UploadResponse();
                foreach (var content in provider.Contents)
                {
                    
                    var KeyName = content.Headers.ContentDisposition.Name.Trim('\"');
                    if (KeyName.Contains("photo"))
                    {
                        var fileName = content.Headers.ContentDisposition.FileName.Trim('\"');
                        string[] fileary = fileName.Split('.');
                        string uploadfilename = username+DateTime.Now.ToFileTime() + "." + fileary[1];
                        var fileBytes = await content.ReadAsByteArrayAsync();

                        var outputPath = Path.Combine(root, uploadfilename);
                        using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            await output.WriteAsync(fileBytes, 0, fileBytes.Length);
                        }
                        var q = from p in db.Members where p.UserName == username select p;
                        foreach (var p in q)
                        {
                            ;
                            p.PicName = username + DateTime.Now.ToFileTime();//檔名
                            p.FileName = fileary[1];//副檔名
                        }

                        db.SaveChanges();
                    }
                    else 
                    {
                        
                        continue;
                    }
                   
                    //uploadResponse.Names.Add(fileName);
                    //uploadResponse.FileNames.Add(outputPath);
                    //uploadResponse.ContentTypes.Add(content.Headers.ContentType.MediaType);
                }

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

        
    }
}
