﻿using System;
using System.Collections;
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
using testdatamodel.listclass;
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
        /// <summary>
        /// 更改會員名字
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="name">會員更新名字</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeName(string username,string name)
        {
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return NotFound();
            }
            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Name = name;
            }

            db.SaveChanges();
            return Ok(new {status = "success"});
        }
        /// <summary>
        /// 更改會員敘述
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="introduction">會員敘述</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeInfo(string username, string introduction)
        {
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return NotFound();
            }

            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Introduction = introduction;
            }

            db.SaveChanges();
            return Ok(new { status = "success" });
        }
        /// <summary>
        /// 更改會員信箱
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="email">會員信箱</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeEmail(string username, string email)
        {
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return NotFound();
            }

            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Email = email;
            }

            db.SaveChanges();
            return Ok(new { status = "success" });
        }
        /// <summary>
        /// 是否公開會員收藏文章
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="opencollect">是否公開</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChagneOpenCollect(string username, bool opencollect)
        {
            var data = db.Members.FirstOrDefault(x => x.UserName == username);
            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Opencollectarticles = opencollect;
            }

            db.SaveChanges();
            return Ok(new {status = "sucess"});
        }
        /// <summary>
        /// 找到會員收藏文章的數量
        /// </summary>
        /// <param name="memberid">會員ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult CollectArticlenumber(int memberid)
        {
            var memberdata = db.Members.FirstOrDefault(x => x.ID == memberid);
            if (memberdata == null)
            {
                return NotFound();
            }
            var dataart = memberdata.Articles.Count;
            var dataartN = memberdata.ArticleNormals.Count;
            var artcount = dataart + dataartN;
            return Ok(new {memberID = memberid, articlecount = artcount});
        }
        /// <summary>
        /// 會員的收藏文章列表
        /// </summary>
        /// <param name="userid">會員ID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAllMemberarticle(int userid)
        {
            var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
            if (memberdata == null)
            {
                return NotFound();
            }

            var dataart = memberdata.Articles.ToList();
            var dataartn = memberdata.ArticleNormals.ToList();
            List<NewArticle> arrayList = new List<NewArticle>();

            foreach (var content in dataart)
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

            foreach (var content in dataartn)
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
            var newArticles = arrayList.OrderByDescending(x => x.InitDateTime);
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
            return Ok(result);
        }
       
        /// <summary>
        /// 取得作者發布的文章數量
        /// </summary>
        /// <param name="authorname">作者的帳號</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMemberartnumber(string authorname)
        {
            var artdata = from q in db.Articles
                where (q.UserName == authorname & q.IsPush == true)
                select q;
            var norartdata = from q in db.ArticleNormals
                          where (q.UserName == authorname & q.IsPush == true)
                                   select q;
            int number = artdata.Count() + norartdata.Count();
            return Ok(new {status = "sucess", artcount = number});
        }
        /// <summary>
        /// 查詢作者蒐藏的文章
        /// </summary>
        /// <param name="authorusername">作者帳號名稱</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Collectauthorarticle(string authorusername)
        {
            var memberdata = from q in db.Members
                where (q.UserName == authorusername & q.Opencollectarticles == true)
                select q;

            int memberid = 0;
            foreach (var str in memberdata)
            {
                memberid = str.ID;
            }
            var authordata = db.Members.FirstOrDefault(m=>m.ID == memberid);
            if (authordata == null)
            {
                return NotFound();
            }
            var artdata = authordata.Articles.ToList();
            var noratrdata = authordata.ArticleNormals.ToList();
            List<NewArticle> arrayList = new List<NewArticle>();

            foreach (var content in artdata)
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

            foreach (var content in noratrdata)
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
            var newArticles = arrayList.OrderByDescending(x => x.InitDateTime);
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
            return Ok(result);
        }
    }
}
