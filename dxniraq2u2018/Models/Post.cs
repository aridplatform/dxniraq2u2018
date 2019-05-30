using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "طول العنوان يجب ان يتراوح بين 20 حرف و 100 حرف", MinimumLength = 25)]
        [Display(Name = "العنوان")]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "يجب ألا يتجاوز المحتوى عن 900 كلمة ولايقل عن 50 كلمة", MinimumLength = 500)]
        [Display(Name = "المحتوى")]
        public string Body { get; set; }

        [Display(Name = "تاريخ النشر")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateTime { get; set; }

        [Display(Name = "السماح بالتعليقات")]
        public bool IsCommentsAllowed { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة")]
        public string Image { get; set; }

        [StringLength(100)]
        [Display(Name = "الملف المرفق")]
        public string File { get; set; }

        [Display(Name = "تمت الموافقة")]
        public bool IsApproved { get; set; }

        [Display(Name = "اخفاء المنشور")]
        public bool IsHidden { get; set; }

        [Display(Name = "مميز")]
        public bool IsFeatured { get; set; }

        [Display(Name = "هدية لافضل اجابة")]
        public bool IsGifted { get; set; }

        [Display(Name = "عدد القراء")]
        public int Reads { get; set; }

        [Display(Name = "حذف المنشور")]
        public bool IsDeleted { get; set; }

        [Display(Name = "المجتمع")]
        public int CommunityId { get; set; }
        [Display(Name = "المجتمع")]
        public Community Community { get; set; }

        [Display(Name = "الناشر")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [Display(Name = "الناشر")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(100, ErrorMessage = "الكلمات المفتاحية قصيرة", MinimumLength = 5)]
        [Required(ErrorMessage = "حقل الكلمات المفتاحية ضروري")]
        [Display(Name = "كلمات مفتاحية")]
        public string Tags { get; set; }

        [Display(Name = "نوع المنشور")]
        public GroupPostType PostType { get; set; }

        [Display(Name = "طلب النشر في المدونة الرئيسة")]
        public bool IsPublishRequest { get; set; }

        [Display(Name = "حالة طلب النشر")]
        public bool PublishRequestStatus { get; set; }

    }
}
