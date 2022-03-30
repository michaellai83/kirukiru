using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using testdatamodel.Models;
using testdatamodel.Secret;

namespace testdatamodel.Areas.Back.Controllers
{
    public class BackMemberController : Controller
    {
        ProjectDb db = new ProjectDb();
        // GET: Back/BackMember
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewArticle()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddNewArticle(string title,string photo,string ContentText)
        {
            var data =GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberpic = memberData[2];
            BackArticle backArticle = new BackArticle();
            backArticle.BackmemberID = Convert.ToInt32(memberId);
            backArticle.BackMemberPic = memberpic;
            backArticle.Main = ContentText;
            backArticle.Title = title;
            backArticle.Titlepic = photo;
            backArticle.IniDateTime = DateTime.Now;
            db.BackArticles.Add(backArticle);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult UploadPhoto()
        {
            string fileName = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files["photo"];
                var filedata = f.FileName.Split('.');
                var fileFile = filedata[1];
                fileName = "FF"+ DateTime.Now.ToFileTime().ToString() + "." + fileFile;
                f.SaveAs(@"C:\Users\賴彥融\Desktop\Project\Project\testdatamodel\testdatamodel\Pic\"+fileName);
            }

            TempData["Photo"] = fileName;
            return RedirectToAction("AddNewArticle", "BackMember");
            //return Content(fileName);
        }

        public static string GetUserData()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = System.Web.HttpContext.Current.User.Identity as FormsIdentity;

                FormsAuthenticationTicket ticked = id.Ticket;
                var userinfo = id.Ticket.UserData;
                return userinfo;
            }

            return "";
        }

    }
}