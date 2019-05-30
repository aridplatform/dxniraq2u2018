using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace dxniraq2u2018.Models
{
    public class BranchAdvertismentScreenk
    
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "الحالة")]
        public bool Status { get; set; }

        [Display(Name = "العداد")]
        public int Timer { get; set; }

        [StringLength(500)]
              [Display(Name = "العنوان")]
        public string Subject { get; set; }

       
        [Display(Name = "محتوى الصفحة")]
        [StringLength(2500)]
        public string Body { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ الاضافة")]
        public DateTime PostDate { get; set; }

    }
}
