using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class LectureRegistration
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "المحاضرة")]
        public int LectureId { get; set; }
        [Display(Name = "المحاضرة")]
        public virtual Lecture Lecture { get; set; }

        [Display(Name = "العضو")]
        public string UserId { get; set; }
        [Display(Name = "العضو")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
    }
}
