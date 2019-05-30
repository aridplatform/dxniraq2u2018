using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class ApplicationUserViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}
