using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Links")] //tên của bảng
    public class Links
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public int TableID { get; set; }
        public string Type { get; set; }


    }
}