using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class InvoiceViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public Invoice Invoice { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
    }
}
