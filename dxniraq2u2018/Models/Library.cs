using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Library
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "اسم الملف")]
        public string FileName { get; set; }

        [Display(Name = "نوع الملف")]
        public int FileType { get; set; }

        [StringLength(450)]
        [Display(Name = "المسار")]
        public string UrlPath { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(1000)]
        [Display(Name = "نبذة")]
        public string Description { get; set; }

        [Display(Name = "تاريح الانشاء")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
    }
}
