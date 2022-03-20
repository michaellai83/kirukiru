


using System;
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
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.ModelBinding;
using testdatamodel.Email;
using testdatamodel.JWT;
using testdatamodel.listclass;
using testdatamodel.Models;
using testdatamodel.PutData;
using testdatamodel.Secret;


namespace testdatamodel.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    /// <summary>
    /// 會員資料處理
    /// </summary>
    public class MemberController : ApiController
    {
        ProjectDb db = new ProjectDb();
        string picpath = WebConfigurationManager.ConnectionStrings["PicturePath"].ConnectionString;
        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="data">會員資料</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CreatMember([FromBody] DataMember data)
        {
            PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
            HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(data.PassWord, 64, SHA256.Create());

            //string str = Sex.boy.ToString();


            if (data != null)
            {
                string sqlusername = data.UserName;
                var membername = db.Members.FirstOrDefault(x => x.UserName == sqlusername);
                if (membername == null)
                {
                    var emailidentify = "Kiru" + DateTime.Now.ToFileTime();


                    Member member = new Member();
                    member.UserName = data.UserName;
                    member.PassWord = hashResultSha256.Digest;
                    member.PasswordSalt = hashResultSha256.Salt;
                    member.Name = data.Name;
                    member.Email = data.Email;
                    member.initDate = DateTime.Now;
                    member.Isidentify = false;
                    member.ArticlecategoryId = data.ArticlecategoryId;
                    member.PicName = "origin";
                    member.FileName = "jpg";
                    member.Emailidentify = emailidentify;
                    db.Members.Add(member);
                    db.SaveChanges();
                    
                    var MemberName = data.Name;
                    var memberemail = data.Email;
                    var id = db.Members.FirstOrDefault(x => x.UserName == sqlusername).ID;
                    Sendmail.Sendemail(id,sqlusername,MemberName,memberemail, emailidentify);
                    return Ok(new
                    {
                        success = true,
                        message = "註冊成功"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "已有此帳號"
                    });
                }

            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "8888"
                });
            }


        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="login">登入表單</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] Logintable login)
        {
            var username = login.Username;
            var password = login.Password;
            var Isusername = db.Members.FirstOrDefault(x=>x.UserName == username);



            if (Isusername == null )
            {
                return Ok(new
                {
                    success = false,
                    message = "帳號密碼錯誤"
                });
            }
            else if (Isusername.Isidentify == false)
            {
                return Ok(new
                {
                    success = false,
                    message = "信箱沒有驗證"
                });

            }
            else
            {
                string Rightpassword = HashWithSaltResult(password, Isusername.PasswordSalt, SHA256.Create()).Digest.ToString();
                if (Isusername.PassWord == Rightpassword)
                {
                    JwtAuthUtil jwt = new JwtAuthUtil();
                    var artlog = db.Articlecategory.FirstOrDefault(m => m.Id == Isusername.ArticlecategoryId);
                    string pic = Isusername.PicName + "." + Isusername.FileName;
                    var order = Isusername.Orderlists.Where(x => x.Issuccess == true).ToList();
                    ArrayList orderlist = new ArrayList();
                    foreach (var q in order)
                    {
                        var resultdata = new
                        {
                            AuthorName = q.AuthorName
                        };
                        orderlist.Add(resultdata);
                    }
                    var data = new
                    {
                        Username = Isusername.UserName,
                        Isusername.Name,
                        Userpic = pic,
                        Isusername.Email,
                        Isusername.Introduction,
                        Hobby = artlog.Name,
                        Subscription = orderlist

                    };

                    var result = new
                    {
                        success = true,
                        token = jwt.GenerateToken(Isusername.ID, Isusername.UserName, Isusername.Name),
                        data = data
                    };
                    return Ok(result);
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        message = "帳號密碼錯誤"
                    });
                }
                
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
                string password = "000000";
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
                    success = true,
                    message = $"密碼為{password},記得登入修改密碼"
                };
                return Ok(result);


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "查無此帳號"
                });
            }
        }
        /// <summary>
        /// 更改密碼
        /// </summary>
        /// <param name="changePassworddata">會員輸入帳密表單(O_Password為舊密碼N_Password為新密碼</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangPassword(ChangePassword changePassworddata)
        {
            var username = changePassworddata.Username;
            var O_password = changePassworddata.O_Password;
            var password = changePassworddata.N_Password;
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
                        success = true,
                        message = "更改完成"
                    };
                    return Ok(result);
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "密碼錯誤"
                    });
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
        /// <param name="name">會員更新名字</param>
        /// <param name="username">會員帳號</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeName(string name, string username)
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
            return Ok(new
            {
                success = true,
                message = "更改完成"
            });
        }
        /// <summary>
        /// 更改會員敘述
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="introduction">會員敘述</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeInfo(string username, string introduction)
        {
            
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有此帳號"
                });
            }

            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Introduction = introduction;
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "更改完成"
            });
        }
        /// <summary>
        /// 更改會員信箱
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="email">會員信箱</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeEmail(string username, string email)
        {
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }

            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Email = email;
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "更改完成"
            });
        }
        /// <summary>
        /// 是否公開會員收藏文章
        /// </summary>
        /// <param name="username">會員帳號</param>
        /// <param name="opencollect">是否公開</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChagneOpenCollect(string username, bool opencollect)
        {
            var data = db.Members.FirstOrDefault(x => x.UserName == username);
            var q = from p in db.Members where p.UserName == username select p;
            foreach (var p in q)
            {
                p.Opencollectarticles = opencollect;
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }
        /// <summary>
        /// 找到會員收藏文章的數量
        /// </summary>
        /// <param name="memberid">會員ID</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetArticlenumber(int memberid)
        {
            var memberdata = db.Members.FirstOrDefault(x => x.ID == memberid);
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message="沒有此會員"
                });
            }
            var dataart = memberdata.Articles.Count;
            var dataartN = memberdata.ArticleNormals.Count;
            var artcount = dataart + dataartN;
            return Ok(new
            {
                success = true,
                articlecount = artcount
            });
        }

        /// <summary>
        /// 會員收藏的切切文章列表
        /// </summary>
        /// <param name="userid">使用者ID</param>
        /// <param name="Nowpage">現在頁面(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        //[Route("api/member/article")]
        //[HttpGet]
        //[JwtAuthFilter]
        //public IHttpActionResult GetAllMemberarticle(int userid, int Nowpage, int showcount)
        //{
        //    var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
        //    if (memberdata == null)
        //    {
        //        return Ok(new
        //        {
        //            success = false,
        //            message = "沒有此帳號"
        //        });
        //    }

        //    var dataart = memberdata.Articles.ToList();
        //    //var dataartn = memberdata.ArticleNormals.ToList();
        //    List<NewArticle> arrayList = new List<NewArticle>();

        //    foreach (var content in dataart)
        //    {
        //        NewArticle newartary = new NewArticle();
        //        newartary.ArticleID = content.ID;
        //        newartary.UserName = content.UserName;
        //        newartary.Title = content.Title;
        //        newartary.Articlecategory = content.Articlecategory.Name;
        //        newartary.Lovecount = content.Lovecount;
        //        newartary.InitDateTime = content.InitDate;

        //        arrayList.Add(newartary);
        //    }

        //    //foreach (var content in dataartn)
        //    //{

        //    //    NewArticle newartary = new NewArticle();
        //    //    newartary.ArticleID = content.ID;
        //    //    newartary.UserName = content.UserName;
        //    //    newartary.Title = content.Title;
        //    //    newartary.Articlecategory = content.Articlecategory.Name;
        //    //    newartary.Lovecount = content.Lovecount;
        //    //    newartary.InitDateTime = content.InitDate;

        //    //    arrayList.Add(newartary);


        //    //}

        //    int total = arrayList.Count;
        //    if (Nowpage == 1)
        //    {
        //        total = arrayList.Count();
        //        var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
        //        ArrayList result = new ArrayList();
        //        foreach (var str in newArticles)
        //        {
        //            var resultdata = new
        //            {
        //                str.ArticleID,
        //                str.UserName,
        //                str.Title,
        //                str.Articlecategory,
        //                str.Lovecount,
        //                str.InitDateTime
        //            };
        //            result.Add(resultdata);
        //        }
        //        return Ok(new
        //        {
        //            success=true,
        //            total = total,
        //            data=result
        //        });
        //    }
        //    else
        //    {
        //        var page = (Nowpage - 1) * showcount;
        //        var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
        //        ArrayList result = new ArrayList();
        //        foreach (var str in newArticles)
        //        {
        //            var resultdata = new
        //            {
        //                str.ArticleID,
        //                str.UserName,
        //                str.Title,
        //                str.Articlecategory,
        //                str.Lovecount,
        //                str.InitDateTime
        //            };
        //            result.Add(resultdata);
        //        }
        //        return Ok(new
        //        {
        //            success = true,
        //            total = total,
        //            data = result
        //        });
        //    }

        //}
        /// <summary>
        /// 會員收藏的一般文章列表
        /// </summary>
        /// <param name="userid">會員ID</param>
        /// <param name="Nowpage">現在頁數(預設為1)</param>
        /// <param name="showcount">顯示幾筆</param>
        /// <returns></returns>
        //[Route("api/member/normal")]
        //[HttpGet]
        //[JwtAuthFilter]
        //public IHttpActionResult GetAllMemberNormalarticle(int userid, int Nowpage, int showcount)
        //{
        //    var memberdata = db.Members.FirstOrDefault(x => x.ID == userid);
        //    if (memberdata == null)
        //    {
        //        return Ok(new
        //        {
        //            success = false,
        //            message = "沒有此帳號"
        //        });
        //    }

        //    var dataart = memberdata.Articles.ToList();
        //    var dataartn = memberdata.ArticleNormals.ToList();
        //    List<NewArticle> arrayList = new List<NewArticle>();


        //    foreach (var content in dataartn)
        //    {

        //        NewArticle newartary = new NewArticle();
        //        newartary.ArticleID = content.ID;
        //        newartary.UserName = content.UserName;
        //        newartary.Title = content.Title;
        //        newartary.Articlecategory = content.Articlecategory.Name;
        //        newartary.Lovecount = content.Lovecount;
        //        newartary.InitDateTime = content.InitDate;

        //        arrayList.Add(newartary);


        //    }

        //    int total = arrayList.Count;
        //    if (Nowpage == 1)
        //    {
        //        total = arrayList.Count();
        //        var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Take(showcount);
        //        ArrayList result = new ArrayList();
        //        foreach (var str in newArticles)
        //        {
        //            var resultdata = new
        //            {
        //                str.ArticleID,
        //                str.UserName,
        //                str.Title,
        //                str.Articlecategory,
        //                str.Lovecount,
        //                str.InitDateTime
        //            };
        //            result.Add(resultdata);
        //        }
        //        return Ok(new
        //        {
        //            success=true,
        //            total = total,
        //            data=result
        //        });
        //    }
        //    else
        //    {
        //        var page = (Nowpage - 1) * showcount;
        //        var newArticles = arrayList.OrderByDescending(x => x.InitDateTime).Skip(page).Take(showcount);
        //        ArrayList result = new ArrayList();
        //        foreach (var str in newArticles)
        //        {
        //            var resultdata = new
        //            {
        //                str.ArticleID,
        //                str.UserName,
        //                str.Title,
        //                str.Articlecategory,
        //                str.Lovecount,
        //                str.InitDateTime
        //            };
        //            result.Add(resultdata);
        //        }
        //        return Ok(new
        //        {
        //            success = true,
        //            total = total,
        //            data = result
        //        });
        //    }

        //}
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
            return Ok(new { status = "success", artcount = number });
        }
        /// <summary>
        /// 查詢作者蒐藏的文章
        /// </summary>
        /// <param name="authorusername">作者帳號名稱</param>
        /// <param name="pageNow">現在頁面(預設為1)</param>
        /// <param name="totalpagecount">全部筆數(預設為0)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Collectauthorarticle(string authorusername, int pageNow, int totalpagecount, int showcount)
        {
            var memberdata = from q in db.Members
                             where (q.UserName == authorusername & q.Opencollectarticles == true)
                             select q;

            int memberid = 0;
            foreach (var str in memberdata)
            {
                memberid = str.ID;
            }
            var authordata = db.Members.FirstOrDefault(m => m.ID == memberid);
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

            if (totalpagecount == 0)
            {
                totalpagecount = arrayList.Count();
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
                return Ok(new { total = totalpagecount, result });
            }
            else
            {
                var page = (pageNow - 1) * showcount;
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
                return Ok(result);
            }

        }
        /// <summary>
        /// 作者所有文章
        /// </summary>
        /// <param name="userid">使用者ID</param>
        /// <param name="pageNow">現在頁面(預設為1)</param>
        /// <param name="totalpagecount">全部筆數(預設為0)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetMyArticles(int userid, int pageNow, int totalpagecount, int showcount)
        {
            var data = db.Members.FirstOrDefault(x => x.ID == userid);
            if (data == null)
            {
                return NotFound();
            }

            var username = data.UserName.ToString();
            var artdata = db.Articles.Where(x => x.UserName == username).ToList();
            var noratrdata = db.ArticleNormals.Where(x => x.UserName == username).ToList();
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

            if (totalpagecount == 0)
            {
                totalpagecount = arrayList.Count();

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
                return Ok(new { total = totalpagecount, result });
            }
            else
            {
                var page = (pageNow - 1) * showcount;
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
                return Ok(result);
            }

        }
    }
}
