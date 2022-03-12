using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
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
                        token = jwt.GenerateToken(Isusername.ID)
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
    }
}
