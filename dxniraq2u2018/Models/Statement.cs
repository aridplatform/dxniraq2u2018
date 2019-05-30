using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Statement
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "التاريخ")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [Display(Name = "الوصل")]
        public int InvoiceId { get; set; }
        [Display(Name = "الوصل")]
        public virtual Invoice Invoice { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "من/الى")]
        public bool Destination { get; set; }

        [Display(Name = "العضو")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "العضو")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
