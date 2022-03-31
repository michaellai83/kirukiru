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
    [Authorize]
    public class BackMemberController : Controller
    {
        ProjectDb db = new ProjectDb();
        // GET: Back/BackMember
      
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewArticle()
        {
            return View();
        }
       
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
            return RedirectToAction("ShowNewArticle");
        }

        public ActionResult ShowNewArticle()
        {
            var data = db.BackArticles.OrderByDescending(x => x.IniDateTime).ToList();
            return View(data);
        }

        public ActionResult DetailNewArticle(int artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID == artId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artTitlePhoto = data.Titlepic;
            var artMain = HttpUtility.HtmlDecode(data.Main);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artTitlePhoto"] = "/images/"+artTitlePhoto;
            ViewData["artMain"] = artMain;
            return View();

        }

        public ActionResult EditArticle(int artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID == artId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artTitlePhoto = data.Titlepic;
            var artMain = HttpUtility.HtmlDecode(data.Main);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artTitlePhoto"] = "/images/" + artTitlePhoto;
            ViewData["artMain"] = artMain;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditArticle(string artId,string title, string ContentText)
        {
            var artID = Convert.ToInt32(artId);
            var data = from q in db.BackArticles
                where q.ID == artID
                select q;
            foreach (var str in data.ToList())
            {
                str.Title = title;
                str.Main = ContentText;
            }

            db.SaveChanges();
            return RedirectToAction("ShowNewArticle");
        }

        public ActionResult DeleteArticle(int artId)
        {
            var data = db.BackArticles.FirstOrDefault(x => x.ID==artId);
            db.BackArticles.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowNewArticle");
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
                var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
                var outputPath = Path.Combine(root, fileName);
                f.SaveAs(outputPath);
                //f.SaveAs(@"C:\Users\賴彥融\Desktop\Project\Project\testdatamodel\testdatamodel\images\"+fileName);
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
  
        public ActionResult GetAdminName()
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberName = memberData[1];
            var memberpic = memberData[2];
            return Content(memberName) ;
        }

 
        public ActionResult GetAdminPic()
        {
            var data = GetUserData();
            var memberData = data.Split('|');
            var memberId = memberData[0];
            var memberName = memberData[1];
            var memberpic = memberData[2];
            return Content(memberpic);
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult AddQA()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddQA(string title, string ContentText)
        {
            BackQA backQa = new BackQA();
            backQa.Title = title;
            backQa.Answer = ContentText;
            backQa.InitDateTime = DateTime.Now;
            db.BackQas.Add(backQa);
            db.SaveChanges();
            return View();
        }

        public ActionResult ShowAllQA()
        {
            var data = db.BackQas.ToList();
            return View(data);
        }

        public ActionResult DetailQA(int qaId)
        {
            var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artMain = HttpUtility.HtmlDecode(data.Answer);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artMain"] = artMain;
            return View();
        }

        public ActionResult DeleteQA(int qaId)
        {
            var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
            db.BackQas.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowAllQA");
        }

        public ActionResult EditQA(int qaId)
        {
            var data = db.BackQas.FirstOrDefault(x => x.ID == qaId);
            var artID = data.ID;
            var artTitle = data.Title;
            var artMain = HttpUtility.HtmlDecode(data.Answer);
            ViewData["artID"] = artID;
            ViewData["artTitle"] = artTitle;
            ViewData["artMain"] = artMain;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditQA(string qaId,string title,string ContentText)
        {
            var QAid = Convert.ToInt32(qaId);
            var data = from q in db.BackQas
                where q.ID == QAid
                select q;
            foreach (var str in data.ToList())
            {
                str.Title = title;
                str.Answer = ContentText;
            }

            db.SaveChanges();
            return RedirectToAction("ShowAllQA");
        }
    }
}