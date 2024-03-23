using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dossiers.Models
{
    public class Cookies
    {
        public static void SaveCookies(Models.Users cust)
        {
            string user = JsonConvert.SerializeObject(cust);
            HttpContext.Current.Response.Cookies["UserDetails"].Value = user;
        }

        public static void DeleteCookies()
        {
            HttpContext.Current.Response.Cookies["UserDetails"].Expires = DateTime.Now.Date.AddDays(-2);
        }


        public static Users UserDetails
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["UserDetails"] != null)
                {
                    try
                    {
                        Users cust = JsonConvert.DeserializeObject<Users>(HttpContext.Current.Request.Cookies["UserDetails"].Value);
                        return cust;
                    }
                    catch (Exception) { }
                }
                return new Users();
            }
        }

        public static Boolean CheckStudent
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["UserDetails"] != null)
                {
                    try
                    {
                        Contxt db = new Contxt();
                        Users cust = JsonConvert.DeserializeObject<Users>(HttpContext.Current.Request.Cookies["UserDetails"].Value);
                        var details = db.Userss.FirstOrDefault(a => a.Username == cust.Username &&  a.Password == cust.Password && a.Role == cust.Role);
                        if(cust.Role == "student") { 
                        if (details != null)
                            return true;
                        }
                    }
                    catch (Exception) { return false; }
                }
                DeleteCookies();
                return false;
            }
        }
        public static Boolean CheckTeacher
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["UserDetails"] != null)
                {
                    try
                    {
                        Contxt db = new Contxt();
                        Users cust = JsonConvert.DeserializeObject<Users>(HttpContext.Current.Request.Cookies["UserDetails"].Value);
                        var details = db.Userss.FirstOrDefault(a => a.Username == cust.Username && a.Password == cust.Password && a.Role == cust.Role);
                        if (cust.Role == "teacher")
                        {
                            if (details != null)
                                return true;
                        }
                    }
                    catch (Exception) { return false; }
                }
                DeleteCookies();
                return false;
            }
        }





    }
}