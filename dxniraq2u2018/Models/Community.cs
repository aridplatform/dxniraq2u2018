using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class Community
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "العضو")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [Display(Name = "العضو")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [StringLength(100)]
        [Display(Name = "الاسم")]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "الاسم المختصر")]
        [Required]
        public string ShortName { get; set; }

        [StringLength(100)]
        [Display(Name = "الصورة الخلفية - اختياري")]
        public string BgImage { get; set; }

        [StringLength(100)]
        [Display(Name = "الشعار")]
        public string Logo { get; set; }

        [Display(Name = "الوصف")]
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
