using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Faq
    {
        public int Id { get; set; }

        [Display(Name = "القسم")]
        public int FaqCategoryId { get; set; }
        [Display(Name = "القسم")]
        public virtual FaqCategory FaqCategory { get; set; }

        [Display(Name = "الترتيب")]
        public int Order { get; set; }

        [Display(Name = "السؤال")]
        [StringLength(450)]
        public string Question { get; set; }

        [Display(Name = "الاجابة")]
        public string Answer { get; set; }

        [Display(Name = "الكلمات المفتاحية")]
        [StringLength(450)]
        public string Meta { get; set; }
    }
}
