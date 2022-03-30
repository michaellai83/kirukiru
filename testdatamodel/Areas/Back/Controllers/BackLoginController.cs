using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using testdatamodel.Models;
using testdatamodel.Secret;

namespace testdatamodel.Areas.Back.Controllers
{
    public class BackLoginController : Controller
    {
        ProjectDb db = new ProjectDb();
        // GET: Back/BackLogin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var member = db.Backmembers.FirstOrDefault(x => x.Username == email);
            if (member == null)
            {
                ViewBag.Err = "帳號密碼錯誤";
                return View();
            }

            var checkpassword = member.Password;
            var checksalt = member.Salt;
            string Rightpassword = HashWithSaltResult(password, checksalt, SHA256.Create()).Digest.ToString();
            if (checkpassword == Rightpassword)
            {
                string userdata = member.ID + "|" + member.Name + "|" + member.Photo; 

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    member.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    true,
                    userdata,
                    FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);

                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index","BackMember");
            }
            else
            {
                ViewBag.Err = "帳號密碼錯誤";
            }

            return View();
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
    }
}