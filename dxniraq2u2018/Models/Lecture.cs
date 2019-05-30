using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان المحاضرة")]
        [StringLength(200)]
        public string Title { get; set; }

        [Display(Name = "المحتوى")]
        [StringLength(4000)]
        public string Content { get; set; }

        [Display(Name = "تاريخ الانعقاد")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [StringLength(450)]
        [Display(Name = "المدرب")]
        public string InstructorId { get; set; }
        [Display(Name = "المدرب")]
        public virtual ApplicationUser Instructor { get; set; }

        [Display(Name = "الفرع")]
        public int BranchId { get; set; }
        [Display(Name = "الفرع")]
        public virtual Branch Branch { get; set; }

        [Display(Name = "عدد المقاعد المتاحة")]
        public int Seats { get; set; }

        [Display(Name = "حالة التسجيل")]
        public bool IsOpen { get; set; }

        [Display(Name = "المستوى")]
        public CourseLevel LevelType { get; set; }

        [Display(Name = "يمكن الالتحاق بها عبر الانترنت")]
        public bool IsOnline { get; set; }

        [Display(Name = "موافقة الادارة")]
        public bool IsAdminApproved { get; set; }

        [Display(Name = "البروشر")]
        [StringLength(100)]
        public string Flyer { get; set; }
    }
}
