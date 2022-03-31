


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
                        UserId = Isusername.ID,
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
        /// <param name="eMail">註冊信箱</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult NewPassWord(string username,string eMail)
        {
            var Ismember = db.Members.FirstOrDefault(m => m.UserName == username );
            var mail = Ismember.Email;
            if (Ismember != null && eMail == mail)
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
        /// <param name="opencollect">是否公開</param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthFilter]
        public IHttpActionResult ChagneOpenCollect(bool opencollect)
        {
            var username = JwtAuthUtil.GetUsername(Request.Headers.Authorization.Parameter);
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
            string authorname = memberUserName;
            var artdata = from q in db.Articles
                          where (q.UserName == authorname & q.IsPush == true)
                          select q;
            if (artdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }
            var norartdata = from q in db.ArticleNormals
                             where (q.UserName == authorname & q.IsPush == true)
                             select q;
            if (norartdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }
            int number = artdata.Count() + norartdata.Count();
            return Ok(new { status = "success", artcount = number });
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
            var memberdata = from q in db.Members
                             where (q.UserName == authorusername & q.Opencollectarticles == true)
                             select q;
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            int memberid = 0;
            foreach (var str in memberdata)
            {
                memberid = str.ID;
            }
            var authordata = db.Members.FirstOrDefault(m => m.ID == memberid);
            if (authordata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有資料或者不公開"
                });
            }
            var artdata = authordata.Articles.ToList();
            List<NewArticle> arrayList = new List<NewArticle>();

            foreach (var content in artdata)
            {

                NewArticle newartary = new NewArticle();
                newartary.artId = content.ID;
                newartary.username = content.UserName;
                newartary.title = content.Title;
                newartary.artArtlog = content.Articlecategory.Name;
                newartary.articlecategoryId = content.ArticlecategoryId;
                newartary.lovecount = content.Lovecount;
                newartary.ArtInitDate = content.InitDate;

                arrayList.Add(newartary);


            }
            var totalpagecount = arrayList.Count();

            if (nowpage == 1)
            {
                
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
                
                return Ok(new
                {
                    success=true,
                    total = totalpagecount, 
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
                
                return Ok(new
                {
                    success=true,
                    total = totalpagecount,
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
            var memberdata = from q in db.Members
                             where (q.UserName == authorusername & q.Opencollectarticles == true)
                             select q;
            if (memberdata == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            int memberid = 0;
            foreach (var str in memberdata)
            {
                memberid = str.ID;
            }
            var authordata = db.Members.FirstOrDefault(m => m.ID == memberid);
            if (authordata == null)
            {
                return Ok(new
                {
                    success=false,
                    message="沒有資料或者不公開"
                });
            }
            var noratrdata = authordata.ArticleNormals.ToList();
            List<NewArticle> arrayList = new List<NewArticle>();


            foreach (var content in noratrdata)
            {

                NewArticle newartary = new NewArticle();
                newartary.artId = content.ID;
                newartary.username = content.UserName;
                newartary.title = content.Title;
                newartary.artArtlog = content.Articlecategory.Name;
                newartary.articlecategoryId = content.ArticlecategoryId;
                newartary.lovecount = content.Lovecount;
                newartary.ArtInitDate = content.InitDate;

                arrayList.Add(newartary);


            }
            int totalpagecount = arrayList.Count();
            if (nowpage == 0)
            {
                
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
                
                return Ok(new
                {
                    success= true,
                    total = totalpagecount,
                    data=newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
               
                return Ok(new
                {
                    suceess=true,
                    total = totalpagecount,
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

            var author = data.Name;
            var authorPic = data.PicName + "." + data.FileName;
            var artdata = db.Articles.Where(x => x.UserName == username).ToList();
            List<ArticleListOutPut> arrayList = new List<ArticleListOutPut>();
            foreach (var content in artdata)
            {
                ArticleListOutPut newartary = new ArticleListOutPut();
                newartary.artId = content.ID;
                newartary.username = content.UserName;
                newartary.author = author;
                newartary.authorPic = authorPic;
                newartary.title = content.Title;
                newartary.firstPhoto = content.FirstPicName + "." + content.FirstPicFileName;
                newartary.introduction = content.Introduction;
                newartary.artArtlog = content.Articlecategory.Name;
                newartary.isFree = content.IsFree;
                newartary.lovecount = content.Lovecount;
                newartary.ArtInitDate = content.InitDate;

                arrayList.Add(newartary);
            }

            //foreach (var content in artdata)
            //{
            //    NewArticle newartary = new NewArticle();
            //    newartary.ArticleID = content.ID;
            //    newartary.UserName = content.UserName;
            //    newartary.Title = content.Title;
            //    newartary.Articlecategory = content.Articlecategory.Name;
            //    newartary.Lovecount = content.Lovecount;
            //    newartary.InitDateTime = content.InitDate;

            //    arrayList.Add(newartary);
            //}
            int totalpagecount = arrayList.Count;
            if (nowpage == 1)
            {

                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);

                return Ok(new
                {
                    success = true,
                    total = totalpagecount,
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = totalpagecount,
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

            var noratrdata = db.ArticleNormals.Where(x => x.UserName == username).ToList();
            List<NewArticle> arrayList = new List<NewArticle>();


            foreach (var content in noratrdata)
            {

                NewArticle newartary = new NewArticle();
                newartary.artId = content.ID;
                newartary.username = content.UserName;
                newartary.title = content.Title;
                newartary.artArtlog = content.Articlecategory.Name;
                newartary.articlecategoryId = content.ArticlecategoryId;
                newartary.lovecount = content.Lovecount;
                newartary.ArtInitDate = content.InitDate;

                arrayList.Add(newartary);


            }

            int totalpagecount = arrayList.Count;
            if (nowpage == 1)
            {

                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Take(showcount);
               
                return Ok(new
                {
                    success=true,
                    total = totalpagecount, 
                    data = newArticles
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var newArticles = arrayList.OrderByDescending(x => x.ArtInitDate).Skip(page).Take(showcount);
               
                return Ok(new
                {
                    success=true,
                    total = totalpagecount,
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
            var q = from p in db.Members where p.UserName == userName select p;
            foreach (var p in q)
            {
                p.PicName = memberPhoto;
                p.FileName = PhotoFileName;
            }

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
            var memberId = db.Members.FirstOrDefault(x => x.UserName == memberUserName).ID;
            if (memberId == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }
            var orderData = from q in db.Orderlists
                where (q.MemberID == memberId && q.Issuccess == true)
                select q;
            var oderlist = orderData.ToList();

            int orderNum = oderlist.Count;
            return Ok(new
            {
                success = true,
                orderNumber = orderNum
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
            var beOrder = from q in db.Orderlists
                where (q.AuthorName == memberUserName && q.Issuccess == true)
                select q;
            if (beOrder == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此帳號"
                });
            }
            var oderlist = beOrder.ToList();
            int beOrderNum = oderlist.Count;
            return Ok(new
            {
                success = true,
                beOrderNumber = beOrderNum
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
            var data = db.Members.FirstOrDefault(x => x.UserName == author);

            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有此作者"
                });
            }

            
           
            var artlog = db.Articlecategory.FirstOrDefault(m => m.Id == data.ArticlecategoryId);
            string pic = data.PicName + "." + data.FileName;
            
           
            var result = new
            {
                UserId = data.ID,
                Username = data.UserName,
                data.Name,
                Userpic = pic,
                data.Introduction,

            };
            return Ok(new
            {
                success = true,
                data = result
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

            var subData = db.Subscriptionplans.FirstOrDefault(x => x.MemberID == userId);
            if (subData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "沒有開通訂閱"
                });
            }
            var result = new
            {
                authorId =userId,
                subId = subData.ID,
                authorUserName = userName,
                subData.Amount
            };
            return Ok(new
            {
                success = true,
                data=result
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
            var data = from q in db.Subscriptionplans
                where q.MemberID == userId
                select q;
            if (data == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "還沒開通訂閱方案"
                });
            }

            foreach (var q in data)
            {
                q.Amount = Amount;
            }

            db.SaveChanges();
            return Ok(new
            {
                success = true,
                message = "修改訂閱金額成功"
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
            var subData = db.Subscriptionplans.FirstOrDefault(x => x.MemberID == memberId);
            if (subData == null)
            {
                return Ok(new
                {
                    success = false,
                    message = "此作者沒有開通訂閱"
                });
            }

            var authorName = memberdata.Name;
            var authorPic = memberdata.PicName + "." + memberdata.FileName;
            
            var result = new
            {
                authorId = memberId,
                author = authorName,
                authorPic = authorPic,
                subId = subData.ID,
                authorUserName = author,
                subData.Amount
            };
            return Ok(new
            {
                success = true,
                data = result
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
            var orderList = db.Orderlists.Where(x => x.MemberID == userId).OrderByDescending(x=>x.InitDateTime).ToList();
            List<OrderMyList> orderOutPut = new List<OrderMyList>();
            int HowPay = 0;
            foreach (var order in orderList)
            {
                OrderMyList orderMyList = new OrderMyList();
                orderMyList.ID = order.ID;
                orderMyList.Author = order.AuthorName;
                orderMyList.Amount = order.Amount;
                orderMyList.IsSuceess = order.Issuccess;
                orderMyList.InitDate = order.InitDateTime;
                orderOutPut.Add(orderMyList);
                HowPay += order.Amount;
            }

            int total = orderOutPut.Count;
            if (nowpage == 1)
            {
                var outPut =orderOutPut.Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    pay=HowPay,
                    data = outPut
                });
            }
            else
            {
                var page = (nowpage - 1) * showcount;
                var outPut = orderOutPut.Skip(page).Take(showcount);
                return Ok(new
                {
                    success = true,
                    total = total,
                    pay = HowPay,
                    data = outPut
                });
            }
        }
    }
}
