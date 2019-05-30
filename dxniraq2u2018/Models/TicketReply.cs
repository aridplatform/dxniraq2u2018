using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class TicketReply
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "الرد")]
        [Required]
        public string Body { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string SupportUserId { get; set; }
        [Display(Name = "المستخدم")]
        public virtual ApplicationUser SupportUser { get; set; }

        [Display(Name = "التذكرة")]
               public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        [Display(Name = "تاريخ الرد")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ReplyDate { get; set; }
    }
}
