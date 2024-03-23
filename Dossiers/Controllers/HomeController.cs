using Dossiers.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Dossiers.Controllers
{
    public class HomeController : Controller
    {
        private Models.Contxt db = new Models.Contxt();

        public ActionResult Index()
        {
            ViewBag.IsStudent = Models.Cookies.CheckStudent;
            ViewBag.IsTeacher = Models.Cookies.CheckTeacher;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Uploads()
        {
            ViewBag.Message = "Your Uploads are here";
            return View();
        }






        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult Edit(Courses users, string DelIds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();

                //string res = Mysps.DeleteOldCourses(users.StID);

                string[] CName = Request["Name"].Split(',').ToArray();
                int[] Cid = Request["Cid"].Split(',').Select(x => int.Parse(x)).ToArray();
                int[] Days = Request["Days"].Split(',').Select(x => int.Parse(x)).ToArray();
                double[] Amount = Request["Amount"].Split(',').Select(x => double.Parse(x)).ToArray();

                for (int i = 0; i < CName.Length; i++)
                {
                    StudentCourses sc = new StudentCourses
                    {

                        Name = CName[i],
                        Days = Days[i],
                        Amount = Amount[i],

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
        public void mailrecieved(string name, string email, string message, Attachment doc)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("enter email of the sender");
            mailMessage.To.Add("enter email u want to recieve otp to");
            mailMessage.Subject = "Test email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<p>Name: " + name + "</p>" + "<p>Email: " + email + "</p>" + "<p>Message: " + message + "</p>";
            if (doc != null)
                mailMessage.Attachments.Add(doc);

            SmtpClient smtpClient = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = new NetworkCredential("enter email of the sender", "enter your pasccode")
            };
            smtpClient.Send(mailMessage);
        }
        public ActionResult Logins(string Username, string Password, string Role)
        {
            //LINQ
            var emp = db.Userss.FirstOrDefault(b => b.Username == Username && b.Password == Password && b.Role == Role);
            if (emp == null)
            {
                TempData["Uid"] = Username;
                TempData["Pwd"] = Password;
                TempData["Rl"] = Role;
                TempData["Msg"] = "Wrong Credentials";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.Cookies.SaveCookies(emp);
                if (Role == "student")
                {
                    return RedirectToAction("Index", "Uploads");
                }
                else
                {
                    return RedirectToAction("Index", "Users");
                }

            }

        }


        public string GenerateOTP(string Username)
        {
            var emp = db.Userss.FirstOrDefault(b => b.Username == Username);
            if(emp != null)
            {
                Random rdm = new Random();
                int OTP = rdm.Next(0001, 9999);
                string result = Mysps.InsertOTP(emp.StID, OTP);
                if (result == "0")
                    return "-1"; //OTP DID NOT SAVE
                else
                {
                   result = SendOTP(OTP.ToString(), emp.Email, emp.FirstName + " " + emp.LastName);
                    if (result == "0")
                        return "-1"; //PROBLEM IN MAIL
                    else return emp.StID.ToString();
                }
            }
            else
            {
                return "0"; //User Does NOT EXISTS
            }   
        }

        public string ResendOTP(int? Uid)
        {
            string Otp = Mysps.GetOtp(Uid);
            if(Otp != "0")
            {
                var emp = db.Userss.Find(Uid);
                return SendOTP(Otp, emp.Email, emp.FirstName + " " + emp.LastName);
            }
            return "0";  
        }

        public string SendOTP(string OTP, string EMail, string Name)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("jokerthegreat515@gmail.com");
                mailMessage.To.Add(EMail);
                mailMessage.Subject = "Login OTP";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<p>Hey <b>" + Name + ",</b><br />Your OTP for login is: <b>" + OTP + "</b></p>";


                SmtpClient smtpClient = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = new NetworkCredential("jokerthegreat515@gmail.com", "uhmwjdvncrxaszkw")
                };
                smtpClient.Send(mailMessage);
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public string AuthOTP(int? Uid, string OTP)
        {
            string MyOtp = Mysps.GetOtp(Uid);
            if(MyOtp == OTP)
            {
                Users emp = db.Userss.Find(Uid);
                Models.Cookies.SaveCookies(emp);
                if (emp.Role == "student")
                    return "1";
                else
                    return "2";
            }
            return "0";
        }

        public ActionResult DeleteCookies()
        {
            Models.Cookies.DeleteCookies();
            return RedirectToAction("Index");
        }

    }
}