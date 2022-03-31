using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using testdatamodel.Bankdata;
using testdatamodel.JWT;
using testdatamodel.Models;
using testdatamodel.Secret;

namespace testdatamodel.Controllers
{
    /// <summary>
    /// 後台專區
    /// </summary>
    public class BackmemberController : ApiController
    {
        ProjectDb db = new ProjectDb();
        /// <summary>
        /// 註冊後台管理員
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="userpassword">密碼</param>
        /// <param name="email">信箱</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreateBackMember(string username,string userpassword,string email,string name)
        {
            PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
            HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(userpassword, 64, SHA256.Create());
            Backmember backmember = new Backmember();
            backmember.Username = username;
            backmember.Password = hashResultSha256.Digest;
            backmember.Salt = hashResultSha256.Salt;
            backmember.Email = email;
            backmember.Name = name;
            backmember.Photo = "origin.jpg";
            backmember.IniDateTime = DateTime.Now;
            db.Backmembers.Add(backmember);
            db.SaveChanges();
            return Ok(new {status = "success" });
        }
        /// <summary>
        /// 後台登入(產生Token)
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult BackLogint(string username, string password)
        {
            var Isusername = db.Backmembers.FirstOrDefault(m => m.Username == username);



            if (Isusername != null)
            {
                string Rightpassword = HashWithSaltResult(password, Isusername.Salt, SHA256.Create()).Digest.ToString();
                if (Isusername.Password == Rightpassword)
                {
                    JwtAuthUtil jwt = new JwtAuthUtil();

                    var result = new
                    {
                        success = "登入成功",
                        token = jwt.GenerateToken(Isusername.ID,Isusername.Username,Isusername.Name)
                    };
                    return Ok(result);
                }
                else
                {
                    return Ok("密碼錯誤");
                }
            }
            else
            {
                return NotFound();
            }
        }
        private HashWithSaltResult HashWithSaltResult(string password, string salt, HashAlgorithm hashAlgo)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
        }
        /// <summary>
        /// 產生QA
        /// </summary>
        /// <param name="title">問題</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreatQA(string title, string answer)
        {
            BackQA backQa = new BackQA();
            backQa.Title = title;
            backQa.Answer = answer;
            backQa.InitDateTime = DateTime.Now;
            db.BackQas.Add(backQa);
            db.SaveChanges();
            return Ok(new {status = "sucess"});
        }
        /// <summary>
        /// 取得所有QA
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Showallqa()
        {
            var data = db.BackQas.ToList();
            return Ok(data);
        }
        /// <summary>
        /// 刪除QA
        /// </summary>
        /// <param name="qaID">QA的ID</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteQA(int qaID)
        {
            var data = db.BackQas.FirstOrDefault(x => x.ID == qaID);
            db.BackQas.Remove(data);
            db.SaveChanges();
            return Ok(new {status = "success" });
        }
        /// <summary>
        /// 信箱驗證
        /// </summary>
        /// <param name="ID">註冊者ID</param>
        /// <param name="email">信箱認證的字串</param>
        /// <returns></returns>
        [Route("checkmail")]
        [HttpGet]
        public IHttpActionResult Checkemail(string ID, string email)
        {
            int id = Convert.ToInt32(ID);
            var member = db.Members.FirstOrDefault(x => x.ID == id);
            if (member == null)
            {
                return Ok<string>("驗證失敗");
            }
            var q = from p in db.Members where p.ID == id select p;
            foreach (var p in q)
            {
                p.Isidentify = true;
            }

            db.SaveChanges();
            //回傳網址
            return Redirect("https://kirukiru.rocket-coding.com/");
        }
        /// <summary>
        /// 新增精選文章
        /// </summary>
        /// <param name="backarticledata">文章所需資料</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Addarticle([FromBody] Backarticledata backarticledata)
        {
            BackArticle backdata = new BackArticle();
            backdata.BackmemberID = backarticledata.backMemberId;
            backdata.Title = backarticledata.title;
            backdata.Main = backarticledata.main;
            backdata.IniDateTime = DateTime.Now;
            db.BackArticles.Add(backdata);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已新增文章"
            });
        }
        /// <summary>
        /// 取得精選文章
        /// </summary>
        /// <returns></returns>
        [Route("api/Backmember/GetAllBackArticles")]
        [HttpGet]
        public IHttpActionResult GetAllBackArticles()
        {
            var data = db.BackArticles.OrderByDescending(x => x.IniDateTime).ToList();
            ArrayList backList = new ArrayList();
            foreach (var str in data)
            {
                var result = new
                {
                    artId = str.ID,
                    author = str.Backmembers.Name,
                    authorPic = str.Backmembers.Photo,
                    title = str.Title,
                    main = str.Main,
                    firstPhoto=str.Titlepic,
                    ArtIniteDate = str.IniDateTime
                };
                backList.Add(result);
            }

            return Ok(new
            {
                success=true,
                data=backList
            });
        }
    }
}
