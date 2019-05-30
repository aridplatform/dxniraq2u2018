using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "الاسم العربي")]
        [StringLength(50, ErrorMessage = "الاسم قصير", MinimumLength = 6)]
        public string ArName { get; set; }

        [Display(Name = "الاسم بالانجليزي")]
        [StringLength(50, ErrorMessage = "الاسم قصير", MinimumLength = 6)]
        public string EnName { get; set; }

        [StringLength(450)]
        [Display(Name = "رقم عضوية الراعي")]
        public string SponsorId { get; set; }

        [StringLength(450)]
        [Display(Name = "DXN")]
        public string DxnId { get; set; }

        [Display(Name = "درجة العضوية")]
        public int MemberTypeId { get; set; }
        public virtual MemberType MemberType { get; set; }

        [Display(Name = "المدينة")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public int CityId { get; set; }
        [Display(Name = "المدينة")]
        public virtual City City { get; set; }

        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public GenderType Gender { get; set; }

        [ScaffoldColumn(false)]
        public decimal Balance { get; set; }

        [Display(Name = "البريد البديل")]
        [EmailAddress(ErrorMessage = "EmailError")]
        [StringLength(100)]
        public string SecondEmail { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [ScaffoldColumn(false)]
        [DataType(DataType.DateTime)]
        public DateTime RegDate { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateofBirth { get; set; }

        [Display(Name = "الصورة الشخصية")]
        [StringLength(500, ErrorMessage = "MinMaxTextFieldError", MinimumLength = 0)]
        public string ProfileImage { get; set; }

        [ScaffoldColumn(false)]
        public int Points { get; set; }

        [Display(Name = "مدرب")]
        public bool IsInstructor { get; set; }

        [Display(Name = "مدير فرع")]
        public bool IsBranchManager { get; set; }

        [Display(Name = "موظف")]
        public bool IsBranchEmployee { get; set; }

        [Display(Name = "سائق")]
        public bool IsDriver { get; set; }

        [Display(Name = "أقرب فرع")]
        public int BranchId { get; set; }
        [Display(Name = "أقرب فرع")]
        public virtual Branch Branch { get; set; }

        [Display(Name = "عضو في الشركة")]
        public bool IsDXNMember { get; set; }
    }
}
