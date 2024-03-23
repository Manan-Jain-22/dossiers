using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dossiers.Models;

namespace Dossiers.Controllers
{
    public class UsersController : Controller
    {
        private Contxt db = new Contxt();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Userss.Where(a => a.Role == "student").ToList());
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Docs(int? id)
        {
            return View(db.Uploadss.Where(a => a.sid == id).ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Userss.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            if (ModelState.IsValid)
            {
                db.Userss.Add(users);
                db.SaveChanges();

                int? sid = users.StID;
                string[] CName = Request["Name"].Split(',').ToArray();
                int[] Days = Request["Days"].Split(',').Select(x => int.Parse(x)).ToArray();
                double[] Amount = Request["Amount"].Split(',').Select(x => double.Parse(x)).ToArray();

                List<StudentCourses> Courses = new List<StudentCourses>();

                for (int i = 0; i < CName.Length; i++)
                {
                    StudentCourses sc = new StudentCourses
                    {
                        Name = CName[i],
                        Days = Days[i],
                        Amount = Amount[i],
                        SId = sid
                    };
                    Courses.Add(sc);
                }

                db.StudentCourses.AddRange(Courses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Userss.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users, string DelIds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();

                //string res = Mysps.DeleteOldCourses(users.StID);
                int? sid = users.StID;
                string[] CName = Request["Name"].Split(',').ToArray();
                int[] Cid = Request["Cid"].Split(',').Select(x => int.Parse(x)).ToArray();
                int[] Days = Request["Days"].Split(',').Select(x => int.Parse(x)).ToArray();
                double[] Amount = Request["Amount"].Split(',').Select(x => double.Parse(x)).ToArray();

                for (int i = 0; i < CName.Length; i++)
                {
                    StudentCourses sc = new StudentCourses
                    {
                        Id = Cid[i],
                        Name = CName[i],
                        Days = Days[i],
                        Amount = Amount[i],
                        SId = sid
                    };

                    if (Cid[i] == 0)
                        db.StudentCourses.Add(sc);
                    else
                        db.Entry(sc).State = EntityState.Modified;
                    db.SaveChanges();
                    //Mysps.Addupd(Cid[i], CName[i], Days[i], Amount[i], sid);
                }

                if (!string.IsNullOrEmpty(DelIds))
                    Mysps.DeleteMultipleCourses(DelIds);
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Userss.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Userss.Find(id);
            db.Userss.Remove(users);
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
