using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        [Display(Name = "البريد الالكتروني")]
        public string Email { get; set; }

        [StringLength(450)]
        [Display(Name = "رقم عضوية الراعي - اختياري")]
        public string SponsorId { get; set; }

        [Required]
        [Compare("Email", ErrorMessage = "البريد الالكتروني في الحقلين لا يتطابقان")]
        [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
        [Display(Name = "اعد كتابة البريد الالكتروني")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        [Display(Name = "المدينة")]
        public int CityId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "كلمة السر قصيرة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "اعد كتابة كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة السر في الحقلين لايتطابقان")]
        public string ConfirmPassword { get; set; }



    }
}
