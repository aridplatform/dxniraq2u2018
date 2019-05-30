using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogPost { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
        public IEnumerable<Product> Product { get; set; }
     
    }
}
