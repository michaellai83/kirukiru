using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using testdatamodel.Models;

namespace testdatamodel.Areas.Back.Controllers
{
    public class BackArticlesController : Controller
    {
        private ProjectDb db = new ProjectDb();

        // GET: Back/BackArticles
        public ActionResult Index()
        {
            var backArticles = db.BackArticles.Include(b => b.Backmembers);
            return View(backArticles.ToList());
        }

        // GET: Back/BackArticles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackArticle backArticle = db.BackArticles.Find(id);
            if (backArticle == null)
            {
                return HttpNotFound();
            }
            return View(backArticle);
        }

        // GET: Back/BackArticles/Create
        public ActionResult Create()
        {
            ViewBag.BackmemberID = new SelectList(db.Backmembers, "ID", "Username");
            return View();
        }

        // POST: Back/BackArticles/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BackmemberID,BackMemberPic,Title,Titlepic,Main,IniDateTime")] BackArticle backArticle)
        {
            if (ModelState.IsValid)
            {
                db.BackArticles.Add(backArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BackmemberID = new SelectList(db.Backmembers, "ID", "Username", backArticle.BackmemberID);
            return View(backArticle);
        }

        // GET: Back/BackArticles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackArticle backArticle = db.BackArticles.Find(id);
            if (backArticle == null)
            {
                return HttpNotFound();
            }
            ViewBag.BackmemberID = new SelectList(db.Backmembers, "ID", "Username", backArticle.BackmemberID);
            return View(backArticle);
        }

        // POST: Back/BackArticles/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BackmemberID,BackMemberPic,Title,Titlepic,Main,IniDateTime")] BackArticle backArticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(backArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BackmemberID = new SelectList(db.Backmembers, "ID", "Username", backArticle.BackmemberID);
            return View(backArticle);
        }

        // GET: Back/BackArticles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BackArticle backArticle = db.BackArticles.Find(id);
            if (backArticle == null)
            {
                return HttpNotFound();
            }
            return View(backArticle);
        }

        // POST: Back/BackArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BackArticle backArticle = db.BackArticles.Find(id);
            db.BackArticles.Remove(backArticle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
