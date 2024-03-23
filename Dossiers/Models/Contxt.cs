using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Dossiers.Models
{
    public class Contxt : DbContext
    {
        public virtual DbSet<Users> Userss { get; set; }
        /*public virtual DbSet<USERSVIEW> Userssview { get; set; }*/
       
        public virtual DbSet<Uploads> Uploadss { get; set; }
        public virtual DbSet<StudentCourses> StudentCourses { get; set; }
        public virtual DbSet<Courses> Coursess { get; set; }

    }
 
}