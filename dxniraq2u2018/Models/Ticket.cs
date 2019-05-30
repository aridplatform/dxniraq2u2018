using dxniraq2u2018.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(500)]
        [Required]
        public string Subject { get; set; }

        [Display(Name = "المحتوى")]
        [Required]
        public string Body { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "تاريخ التذكرة")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime TicketOpenDate { get; set; }

        [Display(Name = "تاريخ الاغلاق")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime TicketCloseDate { get; set; }

        [Display(Name = "حالة التذكرة")]
        public bool Status { get; set; }
    }
}
