using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class AddressViewModel
    {
        public IEnumerable<Address> Addresses { get; set; }
        public Invoice Invoice { get; set; }
        public Address Address { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
    }
}
