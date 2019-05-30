using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Display(Name = "اسم الالبوم")]
        [StringLength(450)]
        public string Subject { get; set; }

        [Display(Name = "وصف الالبوم")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
