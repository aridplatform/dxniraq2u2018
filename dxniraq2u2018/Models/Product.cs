using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "الاسم بالعربي")]
        [Required(ErrorMessage = "RequiredFieldError")]
        [StringLength(300)]
        public string ProductNameArabic { get; set; }

        [Display(Name = "الاسم بالانجليزي")]
        [Required(ErrorMessage = "RequiredFieldError")]
        [StringLength(300)]
        public string ProductNameEnglish { get; set; }

        [Display(Name = "رقم المنتج")]
        [Required(ErrorMessage = "RequiredFieldError")]
        [StringLength(10)]
        public string ProductCode { get; set; }


        [Required(ErrorMessage = "RequiredFieldError")]
        public int PV { get; set; }

        [Required(ErrorMessage = "RequiredFieldError")]
        public decimal SV { get; set; }

        [Display(Name = "الوصف العربي")]
        [StringLength(4000)]
        public string DescArabic { get; set; }

        [Display(Name = "الوصف الانجليزي")]
        [StringLength(4000)]
        public string DescEnglish { get; set; }

        [Display(Name = "سعر الاعضاء")]
        [Required(ErrorMessage = "سعر الاعضاء مطلوب")]
        public decimal MemberPrice { get; set; }

    
        [Required(ErrorMessage = "القسم مطلوب")]
        [Display(Name = "القسم")]
        public int CategoryProductId { get; set; }
        [Display(Name = "القسم")]
        public virtual CategoryProduct CategoryProduct { get; set; }

        [Display(Name = "متوفر في المراكز")]
        public bool IsAvailable { get; set; }

        [Display(Name = "الكمية")]
        [NotMapped]
        public int Quantity { get; } // we can use get only (remove set) to calculate real quantity from different docs

        [Display(Name = "صورة")]
        [StringLength(200)]
        public string Image { get; set; }

        [Display(Name = "الوزن بالغرام")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public int Weight { get; set; }

        [Display(Name = "سعر غير الاعضاء")]
        public decimal NonMemberPrice { get; set; }

        [Display(Name = "الباركود")]
        [StringLength(50)]
        public string BarCode { get; set; }
    }
}
