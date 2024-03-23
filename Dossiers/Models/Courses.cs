using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dossiers.Models
{
    [Table("courses")]
    public class Courses
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
        public float Amount { get; set; } 
        
    }
}