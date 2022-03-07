using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using testdatamodel.JWT;
using testdatamodel.Models;
using testdatamodel.PutData;
using testdatamodel.Secret;
using static testdatamodel.DataCommon;

namespace testdatamodel.Controllers
{
    public class MemberController : ApiController
    {
        ProjectDb db = new ProjectDb();
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="data">會員資料</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreatMember(DataMember data)
        {
            PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
            HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(data.PassWord, 64, SHA256.Create());

            //string str = Sex.boy.ToString();


            if (data != null)
            {
                Member member = new Member();
                member.UserName = data.UserName;
                member.PassWord = hashResultSha256.Digest;
                member.PasswordSalt = hashResultSha256.Salt;
                member.Name = data.Name;
                member.Gender = data.Gender;
                member.Birthday = data.Birthday;
                member.Address = data.Address;
                member.PhoneNumber = data.PhoneNumber;
                member.Email = data.Email;
                member.initDate = DateTime.Now;
                member.Isidentify = false;
                member.ArticlecategoryId = data.ArticlecategoryId;
                member.PicName = "origin";
                member.FileName = "jpg";
                db.Members.Add(member);
                db.SaveChanges();
                return Ok("註冊成功");
            }
            else
            {
                return Ok("8888");
            }


        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Login(string username, string password)
        {

            var Isusername = db.Members.FirstOrDefault(m => m.UserName == username);

           

            if (Isusername != null)
            {
                string Rightpassword = HashWithSaltResult(password, Isusername.PasswordSalt, SHA256.Create()).Digest.ToString();
                if (Isusername.PassWord == Rightpassword)
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
        /// 忘記密碼
        /// </summary>
        /// <param name="username">帳號</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult NewPassWord(string username)
        {
            var Ismember = db.Members.FirstOrDefault(m => m.UserName == username);
            if (Ismember != null)
            {
                string password = "0000";
                PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
                HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(password, 64, SHA256.Create());
                var q = from p in db.Members where p.UserName == username select p;
                foreach (var p in q)
                {
                    p.PassWord = hashResultSha256.Digest;
                    p.PasswordSalt = hashResultSha256.Salt;
                }

                db.SaveChanges();
                var result = new
                {
                    success = "OK",
                    password = password
                };
                return Ok(result);


            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// 更改密碼
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="O_password">舊密碼</param>
        /// <param name="password">新密碼</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangPassword(string username, string O_password, string password)
        {
            var Ismember = db.Members.FirstOrDefault(m => m.UserName == username);
            if (Ismember != null)
            {
                string S_password = HashWithSaltResult(O_password, Ismember.PasswordSalt, SHA256.Create()).Digest.ToString();//建立跟資料庫一樣的密碼

                if (Ismember.PassWord == S_password)
                {
                    PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
                    HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(password, 64, SHA256.Create());
                    var q = from p in db.Members where p.UserName == username select p;
                    foreach (var p in q)
                    {
                        p.PassWord = hashResultSha256.Digest;
                        p.PasswordSalt = hashResultSha256.Salt;
                    }

                    db.SaveChanges();
                    var result = new
                    {
                        success = "OK",

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

        
    }
}
