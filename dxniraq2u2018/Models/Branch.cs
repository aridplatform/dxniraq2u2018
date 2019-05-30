using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "المدينة")]
        [Required(ErrorMessage = "RequiredFieldError")]
        public int CityId { get; set; }
        [Display(Name = "المدينة")]
        public virtual City City { get; set; }

        [StringLength(450)]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [StringLength(450)]
        [Display(Name = "عنوان جوجل")]
        public string GoogleAddress { get; set; }

        [StringLength(450)]
        [Display(Name = "تفاصيل الاتصال")]
        public string Contact { get; set; }

        [StringLength(10)]
        public string Longitude { get; set; }

        [StringLength(10)]
        public string Latitude { get; set; }

        [StringLength(100)]
        [Display(Name = "صورة الفرع")]
        public string BranchPhoto { get; set; }

        [Display(Name = "الحالة")]
        public bool IsActive { get; set; }

    }
}
