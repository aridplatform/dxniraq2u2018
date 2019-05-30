using dxniraq2u2018.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class BlogViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(100)]
        [Display(Name = "الاسم باللغة العربية")]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "الاسم باللغة الانجليزية")]
        [Required]
        public string ShortName { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة الخلفية للمدونة - اختياري")]
        [Required]
        public string BgImage { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة الشخصية")]
        [Required]
        public string Logo { get; set; }

        [Display(Name = "وصف المدونة")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "تاريخ الانشاء")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "نوع المجتمع")]
        [Required]
        public CommunityType CommunityType { get; set; }

        [Display(Name = "التخصص")]
        public int SpecialityId { get; set; }


        [Display(Name = "مستوى الحماية")]
        [Required]
        public SecurityLevel SecurityLevel { get; set; }

        [Display(Name = "السماح بالتعليقات")]
        public Boolean IsCommentsAllowed { get; set; }

        [Display(Name = "مميز")]
        public Boolean IsFeatured { get; set; }

        [Display(Name = "موافقة الادارة")]
        public Boolean IsApproved { get; set; }

        [Display(Name = "ايقاف")]
        public Boolean IsSuspended { get; set; }

        [Display(Name = "الكلمات المفتاحية")]
        [StringLength(250)]
        public string Tags { get; set; }
    }
}
