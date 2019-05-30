using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Page
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان الصفحة")]
        [Required(ErrorMessage = "RequiredFieldError")]
        [StringLength(500)]
        public string Title { get; set; }

        [Display(Name = "المحتوى")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public string Body { get; set; }

        [Display(Name = "الحالة")]
        public bool IsVisible { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "العنوان الانجليزي")]
        [StringLength(500)]
        public string EnglishTitleUrl { get; set; }
    }
}
