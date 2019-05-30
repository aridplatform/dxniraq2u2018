using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class MemberType
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "الاسم العربي")]
        [StringLength(100)]
        public string NameAr { get; set; }

        [Display(Name = "الاسم الانجليزي")]
        [StringLength(100)]
        public string NameEng { get; set; }

        [Display(Name = "المختصر")]
        [StringLength(5)]
        public string TypeAbbreviation { get; set; }

        [Display(Name = "الوصف")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "الصورة")]
        [StringLength(100)]
        public string Image { get; set; }
    }
}
