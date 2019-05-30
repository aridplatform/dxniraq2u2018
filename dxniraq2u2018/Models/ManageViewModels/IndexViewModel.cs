using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Display(Name = "البريد محقق")]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "رقم النقال")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name = "الاسم الثلاثي باللغة العربية")]
        public string ArName { get; set; }

        [Display(Name = "الاسم الثلاثي باللغة الانجليزية")]
        public string EnName { get; set; }

        [Display(Name = "رقم الراعي")]
        public string SponsorId { get; set; }

        [Display(Name = "رقم عضوية DXN")]
        public string DXNId { get; set; }


        [Required]
        [Display(Name = "المدينة")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public GenderType Gender { get; set; }

        [EmailAddress]
        [Display(Name = "البريد البديل")]
        public string SecondEmail { get; set; }


        //[Display(Name = "تاريخ الميلاد")]
        //       [DataType(DataType.Date)]
        //public DateTime DateofBirth { get; set; }

        [Display(Name = "درجة العضوية")]
        public int MemberTypeId { get; set; }

        [Display(Name = "الصورة الشخصية")]
        public string ProfileImage { get; set; }

        [Display(Name = "اقرب فرع اليك")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        [Display(Name = "رابط الدعوة")]
        public string InvitationLink { get; set; }
        
    }
}
