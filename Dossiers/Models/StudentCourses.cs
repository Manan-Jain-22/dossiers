using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dossiers.Models
{
    [Table("StudentCourses")]
    public class StudentCourses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Days { get; set; }
        public double? Amount { get; set; }
        public int? SId { get; set; }
    }
}