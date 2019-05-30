using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class ShipToAddressViewModel
    {
        public BlogPost BlogPost { get; set; }
        public IEnumerable<Address> Address { get; set; }
    }
}
