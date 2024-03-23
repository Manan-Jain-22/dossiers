using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dossiers.Models;
/*using Google.Apis.Analytics.v3.Data;*/

namespace Dossiers.Controllers
{
    public class UploadsController : Controller
    {
        private Contxt db = new Contxt();

        // GET: Uploads
        public ActionResult Index()
        {
            int sid = Models.Cookies.UserDetails.StID;
            return View(db.Uploadss.Where(b=> b.sid == sid).ToList());
        }

        // GET: Uploads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uploads uploads = db.Uploadss.Find(id);
            if (uploads == null)
            {
                return HttpNotFound();
            }
            return View(uploads);
        }

        // GET: Uploads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Uploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Uploads Uploads, HttpPostedFileBase myfile)
        {
            if (ModelState.IsValid)
            {
                if(myfile != null)
                {
                    string ext = Path.GetExtension(myfile.FileName);
                    string NewName = "File" + DateTime.Now.Ticks + ext;
                    string filepath = Path.Combine(Server.MapPath("~/docs"), NewName);
                    myfile.SaveAs(filepath);

                    Uploads.myfile = NewName;
                    Uploads.time = DateTime.Now.Date;
                    Uploads.sid = Models.Cookies.UserDetails.StID;
                    db.Uploadss.Add(Uploads);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }

            return View(Uploads);
        }

        // GET: Uploads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uploads uploads = db.Uploadss.Find(id);
            if (uploads == null)
            {
                return HttpNotFound();
            }
            return View(uploads);
        }

        // POST: Uploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Uploads uploads, HttpPostedFileBase myfileN)
        {
            if (ModelState.IsValid)
            {
                if (myfileN != null)
                {
                    string ext = Path.GetExtension(myfileN.FileName);
                    string NewName = "File" + DateTime.Now.Ticks + ext;
                    string filepath = Path.Combine(Server.MapPath("~/docs"), NewName);
                    myfileN.SaveAs(filepath);
                    uploads.myfile = NewName;
                }

                uploads.time = DateTime.Now.Date;
                db.Entry(uploads).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uploads);
        }

        // GET: Uploads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uploads uploads = db.Uploadss.Find(id);
            if (uploads == null)
            {
                return HttpNotFound();
            }
            return View(uploads);
        }

        // POST: Uploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uploads uploads = db.Uploadss.Find(id);
            db.Uploadss.Remove(uploads);
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
