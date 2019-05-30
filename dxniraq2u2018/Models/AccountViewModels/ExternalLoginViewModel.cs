using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "«·»—Ìœ «·«·ﬂ —Ê‰Ì")]
        public string Email { get; set; }
    }
}
