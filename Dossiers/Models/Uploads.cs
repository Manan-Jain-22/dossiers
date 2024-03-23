using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace Dossiers.Models
{
    [Table("uploads")]
    public class Uploads
    {
        [Key]
        public int id { get; set; }
        public string files { get; set; }
        public string myfile { get; set; }
        public int? sid { get; set; }
        public DateTime? time { get; set; }
    }
}
