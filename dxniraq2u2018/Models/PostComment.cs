using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class PostComment
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(2000, ErrorMessage = "MinMaxTextFieldError", MinimumLength = 30)]
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "التعليق")]
        public string Comment { get; set; }

        [Display(Name = "تاريخ النشر")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateTime { get; set; }

        [Display(Name = "اخفاء التعليق")]
        public bool IsHidden { get; set; }

        [Display(Name = "مميز")]
        public bool IsFeatured { get; set; }

        [Display(Name = "قبول الجواب")]
        public bool IsBestAnswer { get; set; }

        [Display(Name = "حذف التعليق")]
        public bool IsDeleted { get; set; }

        [Display(Name = "الموضوع")]
        public Guid PostId { get; set; }
        [Display(Name = "الموضوع")]
        public virtual Post Post { get; set; }

        [Display(Name = "المستخدم")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(100)]
        [Display(Name = "الملف المرفق")]
        public string File { get; set; }
    }
}
