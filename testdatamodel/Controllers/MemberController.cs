


using Newtonsoft.Json;
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
                    
                    var MemberName = member.Name;
                    var memberemail = member.Email;
                    var id = member.ID;
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
                    var order = Isusername.Orderlists.Where(x => x.Issuccess == true).Select(x=>new
                    {
                        AuthorName = x.AuthorName
                    }).ToList();
                   
                    var data = new
                    {
                        UserId = Isusername.ID,
                        Username = Isusername.UserName,
                        Isusername.Name,
                        Userpic = pic,
                        Isusername.Email,
                        Isusername.Introduction,
                        Hobby = artlog.Name, 
                        isCollect = Isusername.Opencollectarticles,
                        Subscription = order

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
        /// <param name="eMail">註冊信箱</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult NewPassWord(string username,string eMail)
        {
            var Ismember = db.Members.FirstOrDefault(m => m.UserName == username );
            var mail = Ismember.Email;
            if (Ismember != null && eMail == mail)
            {
                string name = Ismember.Name;
                string password = "000000";
                PasswordWithSaltHasher passwordWithSalt = new PasswordWithSaltHasher();
                HashWithSaltResult hashResultSha256 = passwordWithSalt.HashWithSalt(password, 64, SHA256.Create());
                var q = db.Members.Where(x => x.UserName == username);
                //var q = from p in db.Members where p.UserName == username select p;
                foreach (var p in q)
                {
                    p.PassWord = hashResultSha256.Digest;
                    p.PasswordSalt = hashResultSha256.Salt;
                }

                db.SaveChanges();
                Sendmail.SendForgetPassWordsMail(username,name,mail,password);
                var result = new
                {
                    success = true,
                    message = $"以寄信到您的信箱"
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
            var jwtusername = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            if (jwtusername != username)
            {
                return Ok(new
                {
                    success = false,
                    message = "帳號密碼錯誤"
                });
            }
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
                    var q = db.Members.Where(x => x.UserName == username);
                    //var q = from p in db.Members where p.UserName == username select p;
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
                        success = false,
                        message = "帳號密碼錯誤"
                    });
                }
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

        /// <summary>
        /// 更改會員名字
        /// </summary>
        /// <param name="name">會員更新名字</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeName(string name)
        {
            ///從Token取
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter); 
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有此帳號"
                });
            }

            var q = db.Members.Where(x => x.UserName == username).FirstOrDefault();
            //var q = from p in db.Members where p.UserName == username select p;
            //foreach (var p in q)
            //{
            //    p.Name = name;
            //}
            q.Name = name;
            
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
        /// <param name="introduction">會員敘述</param>
        /// <returns></returns>
        [Route("api/Member/ChangeInfo")]
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeInfo([FromBody]string introduction)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有此帳號"
                });
            }

            if (introduction == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有輸入內容"
                });
            }

            var q = db.Members.Where(x => x.UserName == username).FirstOrDefault();
            //var q = from p in db.Members where p.UserName == username select p;
            //foreach (var p in q)
            //{
            //    p.Introduction = introduction;
            //}
            q.Introduction = introduction;
            
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
        /// <param name="email">會員信箱</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChangeEmail(string email)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.Members.FirstOrDefault(m => m.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }

            var q = db.Members.Where(x => x.UserName == username).FirstOrDefault();
            //var q = from p in db.Members where p.UserName == username select p;
            //foreach (var p in q)
            //{
            //    p.Email = email;
            //}
            q.Email = email;
            
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
        /// <param name="opencollect">是否公開</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChagneOpenCollect(bool opencollect)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var data = db.Members.Where(x => x.UserName == username).FirstOrDefault();
            
            //foreach (var p in data)
            //{
            //    p.Opencollectarticles = opencollect;
            //}
            data.Opencollectarticles = opencollect;
            
            db.SaveChanges();
            return Ok(new
            {
                success = true,
            });
        }
        /// <summary>
        /// 找到會員收藏文章的數量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetArticlenumber()
        {
            var memberid = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
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
        /// <param name="memberUserName">會員帳號</param>
        /// <returns></returns>
        [Route("api/Member/getmemberartnumber")]
        [HttpGet]
        public IHttpActionResult GetMemberartnumber(string memberUserName)
        {
           
            var member = db.Members.FirstOrDefault(x => x.UserName == memberUserName);
            if (member == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }

            var artdata = db.Articles.Where(x => x.UserName == memberUserName).Where(x => x.IsPush == true).ToList();
            //var artdata = from q in db.Articles
            //              where (q.UserName == authorname & q.IsPush == true)
            //              select q;

            var norartdata = db.ArticleNormals.Where(x => x.UserName == memberUserName).Where(x => x.IsPush == true).ToList();
            //var norartdata = from q in db.ArticleNormals
            //                 where (q.UserName == authorname & q.IsPush == true)
            //                 select q;
          
            int number = artdata.Count() + norartdata.Count();
            return Ok(new
            {
                status = "success", 
                artcount = number
            });
        }
        /// <summary>
        /// 查詢作者蒐藏的切切文章
        /// </summary>
        /// <param name="authorusername">作者帳號名稱</param>
        /// <param name="nowpage">現在頁面(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Collectauthorarticle(string authorusername, int nowpage ,int showcount)
        {
            //var memberdata = db.Members.Where(x => x.UserName == authorusername)
            //    .Where(x => x.Opencollectarticles == true).FirstOrDefault();
            //var memberdata = from q in db.Members
            //                 where (q.UserName == authorusername & q.Opencollectarticles == true)
            //                 select q;
            var memberdata = db.Members.Where(x => x.UserName == authorusername ).FirstOrDefault();
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            if (memberdata.Opencollectarticles == false)
            {
                return Ok(new
                {
                    success = false,
                    message = "作者不公開收藏"
                });
            }

            int memberid = memberdata.ID;
            //var authordata = db.Members.FirstOrDefault(m => m.ID == memberid).Articles.Select(x=>new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).ToList();
            var authordata = memberdata.Articles
                .Join(db.Members, a => a.UserName, b => b.UserName, (a, b) => new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    artArtlog = a.Articlecategory.Name,
                    articlecategoryId = a.ArticlecategoryId,
                    isFree = a.IsFree,
                    lovecount = a.Lovecount,
                    messageCount = a.Messages.Count,
                    artInitDate = a.InitDate
                }).ToList();
                //Select(x => new
                //{
                //    artId = x.artId,
                //    author = x.author,
                //    authorPic = x.authorPic,
                //    username = x.username,
                //    title = x.title,
                //    firstPhoto = x.firstPhoto,
                //    introduction = x.introduction,
                //    artArtlog = x.artArtlog,
                //    articlecategoryId = x.articlecategoryId,
                //    isFree = x.isFree,
                //    lovecount = x.lovecount,
                //    messageCount = x.messageCount,
                //    artInitDate = x.artInitDate
                //}).ToList();
            //if (authordata.Count == 0)
            //{
            //    return Ok(new
            //    {
            //        success = false,
            //        message = "沒有資料或者不公開"
            //    });
            //}


            if (nowpage == 1)
            {
                
                var newArticles = authordata.OrderByDescending(x => x.artInitDate).Take(showcount);
                
                return Ok(new
                {
                    success=true,
                    total = authordata.Count, 
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = authordata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                
                return Ok(new
                {
                    success=true,
                    total = authordata.Count,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 查詢作者的蒐藏的一般文章
        /// </summary>
        /// <param name="authorusername">作者帳號名稱</param>
        /// <param name="nowpage">現在頁面(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [Route("api/Member/collectnormalarticle")]
        [HttpGet]
        public IHttpActionResult CollectauthorNormalarticle(string authorusername, int nowpage, int showcount)
        {
            //var memberdata = db.Members.Where(x => x.UserName == authorusername)
            //    .Where(x => x.Opencollectarticles == true).FirstOrDefault();
            //var memberdata = from q in db.Members
            //                 where (q.UserName == authorusername & q.Opencollectarticles == true)
            //                 select q;
            var memberdata = db.Members.Where(x => x.UserName == authorusername).FirstOrDefault();
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            if (memberdata.Opencollectarticles == false)
            {
                return Ok(new
                {
                    success = false,
                    message = "作者不公開收藏"
                });
            }
            int memberid = 0;

            //var authordata = db.Members.Where(m => m.ID == memberid).FirstOrDefault().ArticleNormals.Select(x=>new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    messageCount = x.MessageNormals.Count,
            //    artInitDate = x.InitDate
            //}).ToList();
            var authordata = memberdata.ArticleNormals.Join(db.Members,
                a => a.UserName, b => b.UserName, (a, b) => new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                }).ToList();
            //    Select(x => new
            //{
            //    artId = x.artId,
            //    username = x.userName,
            //    author = x.authorName,
            //    authorPic = x.authorPic,
            //    introduction = x.introduction,
            //    title = x.title,
            //    articlecategoryId = x.articlecategoryId,
            //    main = x.main,
            //    isFree = x.isFree,
            //    messageCount = x.messageCount,
            //    lovecount = x.lovecount,
            //    artInitDate = x.artInitDate
            //    }).ToList();
            //if (authordata.Count == 0)
            //{
            //    return Ok(new
            //    {
            //        success=false,
            //        message="沒有資料或者不公開"
            //    });
            //}
            
            if (nowpage == 1)
            {
                
                var newArticles = authordata.OrderByDescending(x => x.artInitDate).Take(showcount);
                
                return Ok(new
                {
                    success= true,
                    total = authordata.Count,
                    data=newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = authordata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
               
                return Ok(new
                {
                    success=true,
                    total = authordata.Count,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 作者所有切切文章
        /// </summary>
        /// <param name="username">作者帳號</param>
        /// <param name="nowpage">現在頁面(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMyArticles( string username, int nowpage, int showcount)
        {
            var data = db.Members.FirstOrDefault(x => x.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有此作者"
                });
            }

            //var artdata = db.Articles.Where(x => x.UserName == username).Where(x => x.IsPush == true).Select(x => new
            //{
            //    artId = x.ID,
            //    author = x.AuthorName,
            //    authorPic = x.AuthorPic,
            //    username = x.UserName,
            //    title = x.Title,
            //    firstPhoto = x.FirstPicName + "." + x.FirstPicFileName,
            //    introduction = x.Introduction,
            //    artArtlog = x.Articlecategory.Name,
            //    articlecategoryId = x.ArticlecategoryId,
            //    isFree = x.IsFree,
            //    lovecount = x.Lovecount,
            //    artInitDate = x.InitDate
            //}).ToList();
            //var author = data.Name;
            //var authorPic = data.PicName + "." + data.FileName;
            //var artdata = from q in db.Articles
            //    where (q.UserName == username && q.IsPush == true)
            //    select q;
            var artdata = db.Articles.Where(x => x.UserName == username && x.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    author = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    title = a.Title,
                    firstPhoto = a.FirstPicName + "." + a.FirstPicFileName,
                    introduction = a.Introduction,
                    artArtlog = a.Articlecategory.Name,
                    articlecategoryId = a.ArticlecategoryId,
                    isFree = a.IsFree,
                    lovecount = a.Lovecount,
                    messageCount = a.Messages.Count,
                    artInitDate = a.InitDate
                }).Select(x => new
            {
                artId = x.artId,
                author = x.author,
                authorPic = x.authorPic,
                username = x.username,
                title = x.title,
                firstPhoto = x.firstPhoto,
                introduction = x.introduction,
                artArtlog = x.artArtlog,
                articlecategoryId = x.articlecategoryId,
                isFree = x.isFree,
                lovecount = x.lovecount,
                messageCount = x.messageCount,
                artInitDate = x.artInitDate
                }).ToList();

            if (nowpage == 1)
            {

                var newArticles = artdata.OrderByDescending(x => x.artInitDate).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = artdata.Count,
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = artdata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = artdata.Count,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 作者所有一般文章
        /// </summary>
        /// <param name="username">作者帳號</param>
        /// <param name="nowpage">現在頁面(預設為1)</param>
        /// <param name="showcount">一頁顯示幾筆資料</param>
        /// <returns></returns>
        [Route("api/Member/getnormalarticles")]
        [HttpGet]
        public IHttpActionResult GetMyNormalArticles( string username, int nowpage, int showcount)
        {
           
            var data = db.Members.FirstOrDefault(x => x.UserName == username);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            //var noratrdata = db.ArticleNormals.Where(x => x.UserName == username).Where(x => x.IsPush == true).Select(
            //    x => new
            //    {
            //        artId = x.ID,
            //        author = x.AuthorName,
            //        authorPic = x.AuthorPic,
            //        username = x.UserName,
            //        title = x.Title,
            //        introduction = x.Introduction,
            //        artArtlog = x.Articlecategory.Name,
            //        articlecategoryId = x.ArticlecategoryId,
            //        isFree = x.IsFree,
            //        lovecount = x.Lovecount,
            //        messageCount = x.MessageNormals.Count,
            //        artInitDate = x.InitDate
            //    }).ToList();
            //var noratrdata = from q in db.ArticleNormals
            //        where (q.UserName == username && q.IsPush == true)
            //                 select q;
            var noratrdata = db.ArticleNormals.Where(x => x.UserName == username && x.IsPush == true)
                .Join(db.Members,a=>a.UserName,b=>b.UserName,(a,b)=>new
                {
                    artId = a.ID,
                    userName = a.UserName,
                    authorName = b.Name,
                    authorPic = b.PicName + "." + b.FileName,
                    introduction = a.Introduction,
                    title = a.Title,
                    articlecategoryId = a.ArticlecategoryId,
                    main = a.Main,
                    isFree = a.IsFree,
                    messageCount = a.MessageNormals.Count,
                    lovecount = a.Lovecount,
                    artInitDate = a.InitDate
                }).Select(
                x => new
                {
                    artId = x.artId,
                    username = x.userName,
                    author = x.authorName,
                    authorPic = x.authorPic,
                    introduction = x.introduction,
                    title = x.title,
                    articlecategoryId = x.articlecategoryId,
                    main = x.main,
                    isFree = x.isFree,
                    messageCount = x.messageCount,
                    lovecount = x.lovecount,
                    artInitDate = x.artInitDate
                }).ToList();


            if (nowpage == 1)
            {

                var newArticles = noratrdata.OrderByDescending(x => x.artInitDate).Take(showcount);
               
                return Ok(new
                {
                    success=true,
                    total = noratrdata.Count, 
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = noratrdata.OrderByDescending(x => x.artInitDate).Skip(page).Take(showcount);
               
                return Ok(new
                {
                    success=true,
                    total = noratrdata.Count,
                    data = newArticles
                });
            }

        }
        /// <summary>
        /// 更改會員大頭貼
        /// </summary>
        /// <param name="Userpic">圖片名稱</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Member/changephoto")]
        [JwtAuthFilter]
        public IHttpActionResult ChangePhoto(string Userpic)
        {
            var userName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
            var memeberdata = db.Members.FirstOrDefault(x => x.UserName == userName);
            if (memeberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "無此作者"
                });
            }

            var photo = Userpic.Split('.');
            string memberPhoto = photo[0];
            string PhotoFileName = photo[1];
            var q = db.Members.FirstOrDefault(x => x.UserName == userName);
            //foreach (var p in q)
            //{
            //    p.PicName = memberPhoto;
            //    p.FileName = PhotoFileName;
            //}
            q.PicName = memberPhoto;
            q.FileName = PhotoFileName;
            
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "修改圖片成功"
            });
        }
        /// <summary>
        /// 取得會員訂閱數量
        /// </summary>
        /// <param name="memberUserName">會員帳號</param>
        /// <returns></returns>
        [Route("api/Member/GetOrderNumber")]
        [HttpGet]
        public IHttpActionResult Getordernumber(string memberUserName)
        {
            var member = db.Members.FirstOrDefault(x => x.UserName == memberUserName);
            if (member == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            int memberId = member.ID;
            var orderData = db.Orderlists.Where(x => x.MemberID == memberId).Where(x => x.Issuccess == true).ToList();
            //var orderData = from q in db.Orderlists
            //    where (q.MemberID == memberId && q.Issuccess == true)
            //    select q;
            
            return Ok(new
            {
                success = true,
                orderNumber = orderData.Count
            });
        }
        /// <summary>
        /// 取得會員被多少人訂閱的數量
        /// </summary>
        /// <param name="memberUserName">會員帳號</param>
        /// <returns></returns>
        [Route("api/Member/GetBeOrder")]
        [HttpGet]
        public IHttpActionResult GetBeordernumber(string memberUserName)
        {
            var beOrder = db.Orderlists.Where(x => x.AuthorName == memberUserName).Where(x => x.Issuccess == true).ToList();
            //var beOrder = from q in db.Orderlists
            //              where (q.AuthorName == memberUserName && q.Issuccess == true)
            //              select q;
            if (beOrder == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }
            return Ok(new
            {
                success = true,
                beOrderNumber = beOrder.Count
            });
        }
        /// <summary>
        /// 回傳作者個人資料
        /// </summary>
        /// <param name="author">會員帳號</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetName(string author)
        {
            var data = db.Members.Where(x => x.UserName == author).Select(x=>new
            {
                UserId = x.ID,
                Username = x.UserName,
                x.Name,
                Userpic = x.PicName + "." + x.FileName,
                x.Introduction,
            }).FirstOrDefault();

            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            
            
            return Ok(new
            {
                success = true,
                data = data
            });
        }
        /// <summary>
        /// 開通會員訂閱方案
        /// </summary>
        /// <returns></returns>
        [Route("api/Member/AddNewSubscriptionplans")]
        [HttpPost]
        [JwtAuthFilter]
        public IHttpActionResult AddNewSubscriptionplans()
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            Subscriptionplan subscriptionplan = new Subscriptionplan();
            subscriptionplan.MemberID = userId;
            subscriptionplan.Amount = "0";
            subscriptionplan.InitDateTime = DateTime.Now;
            db.Subscriptionplans.Add(subscriptionplan);
            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已開通訂閱方案"
            });
        }
        /// <summary>
        /// 取得我的方案
        /// </summary>
        /// <returns></returns>
        [Route("api/Member/GetMySubscriptionplans")]
        [HttpGet]
        [JwtAuthFilter]
        public IHttpActionResult GetMySubscriptionplans()
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);

            var userName = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);

            var subData = db.Subscriptionplans.Where(x => x.MemberID == userId).Select(x=>new
            {
                authorId = x.Members.ID,
                subId = x.ID,
                authorUserName = x.Members.UserName,
                info = x.Introduction,
                amount = x.Amount
            }).FirstOrDefault();
            if (subData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有開通訂閱"
                });
            }
           
            return Ok(new
            {
                success = true,
                data=subData
            });
        }
        /// <summary>
        /// 修改訂閱金額
        /// </summary>
        /// <param name="Amount">訂閱金額</param>
        /// <returns></returns>
        [Route("api/Member/EditSubserciptionplans")]
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult EditSubserciptionplans(string Amount)
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Subscriptionplans.FirstOrDefault(x => x.MemberID == userId);
          
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "還沒開通訂閱方案"
                });
            }

            data.Amount = Amount;
            //foreach (var q in data)
            //{
            //    q.Amount = Amount;
            //}

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "修改訂閱金額成功"
            });
        }
        /// <summary>
        /// 修改訂閱簡介
        /// </summary>
        /// <param name="info">簡介輸入</param>
        /// <returns></returns>
        [Route("api/Member/EditSubInfo")]
        [JwtAuthFilter]
        [HttpPut]
        public IHttpActionResult EditSubInfo([FromBody]string info)
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            var data = db.Subscriptionplans.FirstOrDefault(x => x.MemberID == userId);
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "還沒開通訂閱方案"
                });
            }

            data.Introduction = info;
            //foreach (var q in data)
            //{
            //    q.Introduction = info;
            //}

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "已修改訂閱簡介"
            });
        }
        /// <summary>
        /// 取得作者方案
        /// </summary>
        /// <param name="author">作者帳號</param>
        /// <returns></returns>
        [Route("api/Member/GetAuthorSubscriptionplans")]
        [HttpGet]
        public IHttpActionResult GetAuthorSubscriptionplans(string author)
        {
            var memberdata = db.Members.FirstOrDefault(x => x.UserName == author);
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            int memberId = memberdata.ID;
            var subData = db.Subscriptionplans.Where(x => x.MemberID == memberId).Select(x=>new
            {
                authorId = x.MemberID,
                author = x.Members.Name,
                authorPic = x.Members.PicName+"."+x.Members.FileName,
                subId = x.ID,
                authorUserName = x.Members.UserName,
                info = x.Introduction,
                amount = x.Amount
            }).FirstOrDefault();
            if (subData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "此作者沒有開通訂閱"
                });
            }

            return Ok(new
            {
                success = true,
                data = subData
            });
        }
        /// <summary>
        /// 我的訂閱清單
        /// </summary>
        /// <param name="nowpage">現在頁數(預設1)</param>
        /// <param name="showcount">一頁顯示幾筆</param>
        /// <returns></returns>
        [Route("api/Member/GetMyOrder")]
        [JwtAuthFilter]
        [HttpGet]
        public IHttpActionResult GetMyOrder(int nowpage, int showcount)
        {
            var userId = JwtAuthUtil.GetId(Request.Headers.Authorization.Parameter);
            //var orderList = db.Orderlists.Where(x => x.MemberID == userId).Where(x=>x.Issuccess == true).Select(x=>new
            //{
            //    ID=x.ID,
            //    Author=x.AuthorName,
            //    Amount = x.Amount,
            //    IsSuccess=x.Issuccess,
            //    InitDate=x.InitDateTime
            //}).OrderByDescending(x => x.InitDate).ToList();
            var orderList = db.Orderlists.Where(x => x.MemberID == userId && x.Issuccess == true)
                .Join(db.Members,a=>a.AuthorName,b=>b.UserName,(a,b)=>new
                {
                    ID = a.ID,
                    Author = b.Name,
                    Amount = a.Amount,
                    authorPic = b.PicName + "." + b.FileName,
                    username = b.UserName,
                    IsSuccess = a.Issuccess,
                    InitDate = a.InitDateTime
                }).Select(x => new
            {
                ID = x.ID,
                Author = x.Author,
                authorPic = x.authorPic,
                username = x.username,
                Amount = x.Amount,
                IsSuccess = x.IsSuccess,
                InitDate = x.InitDate
            }).OrderByDescending(x => x.InitDate).ToList();

            int HowPay = 0;
            foreach (var order in orderList)
            {
                if (order.IsSuccess == true)
                {
                    HowPay += order.Amount;
                }
            }

            if (nowpage == 1)
            {
                var outPut = orderList.Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = orderList.Count,
                    pay = HowPay,
                    data = outPut
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var outPut = orderList.Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = orderList.Count,
                    pay = HowPay,
                    data = outPut
                });
            }
        }
        
        //public IHttpActionResult GetMyAmount()
    }
}
