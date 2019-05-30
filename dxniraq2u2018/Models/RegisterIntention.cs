using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class RegisterIntention
    {
        public int Id { get; set; }

        [Display(Name = "رقم العضوية")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "رقم العضوية")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
