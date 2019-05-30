using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الحالية")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "كلمة السر قصيرة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "اعد كتابة كلمة السر ")]
        [Compare("NewPassword", ErrorMessage = "كلمة السر في الحقلين لا يتطابقان")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
