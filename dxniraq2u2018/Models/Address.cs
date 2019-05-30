using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "المدينة")]
        [Required(ErrorMessage = "المدينة مطلوبة")]
        public int CityId { get; set; }
        [Display(Name = "المدينة")]
        public virtual City City { get; set; }

        [Display(Name = "عنوان المستلم")]
        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(500)]
        public string FullAddress { get; set; }

        [Display(Name = "التحقق عبر مدير النظام")]
        [ScaffoldColumn(false)]
        public Boolean IsVerifiedByAdmin { get; set; }

        [Display(Name = "تم التوصيل له من قبل")]
        [ScaffoldColumn(false)]
        public Boolean IsDeliveredBefore { get; }

        [Display(Name = "عنوان جوجل")]
        [ScaffoldColumn(false)]
        [StringLength(200)]
        public string GoogleAddress { get; set; }

        [Display(Name = "اسم المستلم")]
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [StringLength(100)]
        public string ContactPersonName { get; set; }

        [Display(Name = "البريد الالكتروني")]
        [EmailAddress]
              [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "رقم نقال المستلم")]
        [Required(ErrorMessage = "رقم النقال مطلوب")]
        [StringLength(20)]
        public string Mobile { get; set; }

        [Display(Name = "ملاحظات")]
        [StringLength(500)]
        public string UserComment { get; set; }

        [Display(Name = "ملاحظات مدير النظام")]
        [ScaffoldColumn(false)]
        [StringLength(500)]
        public string AdminComment { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [Display(Name = "العنوان الافتراضي")]
        [ScaffoldColumn(false)]
        public bool IsDefault { get; set; }

        [Display(Name = "اعلمني بأي جديد يخص هذا العنوان")]
        [DefaultValue(1)]
        public bool AlertMe { get; set; }

        [Display(Name = "المستخدم")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "المستخدم")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "حفظ العنوان بالاسم")]
        public string AddressName { get; set; }

    }
}
