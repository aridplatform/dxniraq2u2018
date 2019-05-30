using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Testimonial
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "العضو")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [Display(Name = "العضو")]
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "التعليق")]
        [StringLength(5000)]
        public string Content { get; set; }

        [Display(Name = "حالة الظهور")]
        public bool IsVisible { get; set; }

        [Display(Name = "تعليق بارز")]
        public bool IsFeatured { get; set; }
                    }
}
