using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class CartViewModel
    {
        public IEnumerable<ShoppingCartItem> Items { get; set; }
      //  public ShoppingCartItem ShoppingCartItem { get; set; }
    }
}
