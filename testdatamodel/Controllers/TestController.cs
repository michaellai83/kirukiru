using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Script.Serialization;
using Namotion.Reflection;
using Newtonsoft.Json;
using testdatamodel.JWT;
using testdatamodel.Models;
using testdatamodel.PutData;
using testdatamodel.Secret;

namespace testdatamodel.Controllers
{
    //記得取id的時候 不要只打id 要不然容易出錯
    /// <summary>
    /// 測試的API
    /// </summary>
    public class TestController : ApiController
    {
        ProjectDb db = new ProjectDb();

        /// <summary>
        /// 回傳會員資料
        /// </summary>
        /// <returns></returns>
        [Route("GetName")]
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetName()
        {
            var data = JwtAuthUtil.GetuserList(Request.Headers.Authorization.Parameter);
            
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }
            var userName = data.Item2;
            var user = db.Members.Where(m => m.UserName == userName).Join(db.Articlecategory,a=>a.ArticlecategoryId,b=>b.Id,(a,b)=>new
            {
                UserId = a.ID,
                Username = a.UserName,
                Name =a.Name,
                Userpic = a.PicName + "." + a.FileName,
                Email=a.Email,
                Introduction=a.Introduction,
                Hobby = b.Name,
                IsCollect=a.Opencollectarticles,
                Subscription = a.Orderlists.Where(x=>x.Issuccess == true).Select(c=>new
                {
                    AuthorName = c.AuthorName
                })
            }).Select(x=>new
            {
                UserId = x.UserId,
                Username = x.Username,
                Name = x.Name,
                Userpic = x.Userpic,
                Email = x.Email,
                Introduction = x.Introduction,
                Hobby = x.Hobby,
                isCollect=x.IsCollect,
                Subscription = x.Subscription
            }).FirstOrDefault();
            //var artlog = db.Articlecategory.FirstOrDefault(m => m.Id == user.ArticlecategoryId);
            //string pic = user.PicName + "."+user.FileName;
            //var order = user.Orderlists.Where(x => x.Issuccess == true).ToList();
            //ArrayList orderlist = new ArrayList();
            //foreach (var q in order)
            //{
            //    var resultdata = new
            //    {
            //        AuthorName = q.AuthorName
            //    };
            //    orderlist.Add(resultdata);
            //}
            //var result = new
            //{
            //    UserId = user.ID,
            //    Username =user.UserName,
            //    user.Name,
            //    Userpic = pic,
            //    user.Email,
            //    user.Introduction,
            //    Hobby=artlog.Name,
            //    Subscription = orderlist
            //};
            return Ok(new
            {
                success=true,
                data=user
            });
        }
        /// <summary>
        /// 加入文章類別
        /// </summary>
        /// <param name="str">輸入文章類別</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreateArticlecategory(string str)
        {
            Articlecategory articlecategory = new Articlecategory();
            articlecategory.Name = str;
            db.Articlecategory.Add(articlecategory);
            db.SaveChanges();
            
            return Ok(new
            {
                success=true,
                message="添加成功"
            });
        }
        /// <summary>
        /// 查所有文章類別
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetArticlecategory()
        {
            var articlecategory = db.Articlecategory.ToList();
           
            ArrayList result = new ArrayList();
            foreach (var str in articlecategory)
            {
                var data = new {str.Id, str.Name};
                //data.Id =str.Id;
                //data.Name = str.Name;
                result.Add(data);
            }
            //string SqlName = WebConfigurationManager.ConnectionStrings["ProjectDb"].ConnectionString;
            //SqlConnection connection = new SqlConnection(SqlName);
            //// 2.sql語法 (@參數化避免隱碼攻擊)
            //string sqlcheckaccount = "SELECT * FROM Articlecategories ";
            //// 3.創建 command 物件
            //SqlCommand commandcheckaccount = new SqlCommand(sqlcheckaccount, connection);
            //SqlDataAdapter adapter = new SqlDataAdapter(commandcheckaccount);
            //DataTable table = new DataTable();
            //adapter.Fill(table);
            //if (table.Rows.Count > 0)
            //{
            //    return Ok(table);
            //}
            //else
            //{
            //    return NotFound();
            //}

            return Ok(result);
        }
        /// <summary>
        /// 刪除文章類別
        /// </summary>
        /// <param name="Artid">刪除文章類別的id</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteArtlog(int Artid)
        {
            var articlecategory = db.Articlecategory.Where(m => m.Id == Artid).FirstOrDefault();
            db.Articlecategory.Remove(articlecategory);
            db.SaveChanges();
            return Ok(new  {
                success = true,
                message = "刪除成功"
            
            });
        }
        /// <summary>
        /// 找到文章類別
        /// </summary>
        /// <param name="Artid">找到文章類別的ID</param>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult GetArtlog(int Artid)
        {
            try
            {
                var articlog = db.Articlecategory.Where(m => m.Id == Artid).FirstOrDefault();
                //匿名型別
                var result = new
                {
                    Id = articlog.Id,
                    Name = articlog.Name
                };


                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok((ex));
            }
        }
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="data">會員資料</param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult CreatMember(DataMember data)
        //{
        //    PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
        //    HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(data.PassWord, 64, SHA256.Create());



        //    if (data != null)
        //    {
        //        Member member = new Member();
        //        member.UserName = data.UserName;
        //        member.PassWord = hashResultSha256.Digest;
        //        member.PasswordSalt = hashResultSha256.Salt;
        //        member.Name = data.Name;
        //        member.Gender = data.Gender;
        //        member.Birthday = data.Birthday;
        //        member.Address = data.Address;
        //        member.PhoneNumber = data.PhoneNumber;
        //        member.Email = data.Email;
        //        member.initDate = DateTime.Now;
        //        member.Isidentify = false;
        //        member.ArticlecategoryId = data.ArticlecategoryId;
        //        db.Members.Add(member);
        //        db.SaveChanges();
        //        return Ok("註冊成功");
        //    }
        //    else
        //    {
        //        return Ok("8888");
        //    }


        //}
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="username">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        //[HttpGet]
        //public IHttpActionResult Login(string username, string password)
        //{ 

        //    var Isusername = db.Members.FirstOrDefault(m => m.UserName == username);

        //    string Rightpassword = HashWithSaltResult(password, Isusername.PasswordSalt, SHA256.Create()).Digest.ToString();

        //    if (Isusername !=null)
        //    {
        //        if (Isusername.PassWord == Rightpassword)
        //        {
        //            JwtAuthUtil jwt = new JwtAuthUtil();

        //            var result = new
        //            {
        //                success = "登入成功",
        //                token = jwt.GenerateToken(Isusername.ID,Isusername.UserName,Isusername.Name)
        //            };
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return Ok("密碼錯誤");
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

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
            var Ismember = db.Members.FirstOrDefault(m=>m.UserName==username);
            if (Ismember != null)
            {
                string password = "0000";
                PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
                HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(password, 64, SHA256.Create());
                var q =from p in db.Members where p.UserName == username select p;
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
        public IHttpActionResult ChangPassword(string username,string O_password, string password)
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
