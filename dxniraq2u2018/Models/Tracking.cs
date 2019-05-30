using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Tracking
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "رقم الوصل")]
        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        [Display(Name = "العملية")]
        public int ProcessingId { get; set; }

     
    }
}
