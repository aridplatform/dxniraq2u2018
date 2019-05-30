using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class StoreLog
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "المنتج")]
        public int ProductId { get; set; }
        [Display(Name = "المنتج")]
        public Product Product { get; set; }

        [Display(Name = "الفرع")]
        public int BranchId { get; set; }

        [Display(Name = "الكمية")]
        public int Quantity { get; set; }

        [Display(Name = "التاريخ")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [Display(Name = "+ | - ")]
        public bool Status { get; set; }
    }
}
