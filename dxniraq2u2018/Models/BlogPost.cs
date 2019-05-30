using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class BlogPost
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "القسم")]
        public int BlogCategoryId { get; set; }
        [Display(Name = "القسم")]
        public BlogCategory BlogCategory { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        [Display(Name = "نبذة مختصرة")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public string Introduction { get; set; }

        [StringLength(200)]
        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public string Title { get; set; }

        [Required(ErrorMessage = "RequiredFieldError")]
        [Display(Name = "المحتوى")]
        public string Body { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة")]
        public string Image { get; set; }

        [Display(Name = "ظاهر")]
        public bool IsVisible { get; set; }

        [Display(Name = "موضوع مميز")]
        public bool IsFeatured { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [StringLength(450)]
        [Display(Name = "المستخدم")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "المستخدم")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(450)]
        [Display(Name = "الادارة")]
        public string AdminUserId { get; set; }
        [Display(Name = "الادارة")]
        public virtual ApplicationUser AdminUser { get; set; }

        [StringLength(450)]
        [Display(Name = "الصورة المصغرة")]
        public string Imagethumb { get; set; }

        [StringLength(450)]
        [Display(Name = "الكلمات المفتاحية")]
        public string Meta { get; set; }

        [StringLength(450)]
        [Display(Name = "ملف مرفق - اختياري")]
        public string Filelink { get; set; }








    }
}
