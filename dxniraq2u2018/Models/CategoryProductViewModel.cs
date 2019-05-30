using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class CategoryProductViewModel
    {
        public CategoryProduct CategoryProduct { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CategoryProduct> CategoryProducts { get; set; }

        [Display(Name = "عدد المنتجات")]
        public int NoOfItems { get; set; }
    }
}
