using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> Items { get; set; }
      

        public int ProductId { get; set; }
        [Display(Name = "المنتج")]
        public Product Product { get; set; }

        [Display(Name = "العضو")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [StringLength(20)]
        public string DXNId { get; set; }

        [Display(Name = "الكمية")]
        public int Quantity { get; set; }

        [Display(Name = "السعر")]
        [NotMapped]
        public decimal SalePrice
        {
            get
            {
                if (this.Product != null)
                {
                    if (this.ApplicationUser.IsDXNMember)
                    {
                        return this.Product.MemberPrice;
                    }
                    else
                    {
                        return this.Product.NonMemberPrice;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        [NotMapped]
        public decimal SubTotal
        {
            get
            {
                if (this.Product != null)
                {
                    return this.Quantity * this.SalePrice;
                }
                else
                {
                    return 0;
                }
            }
        }

        [NotMapped]
        public int TotalPV
        {
            get
            {
                if (this.Product != null)
                {
                    return this.Quantity * this.Product.PV;
                }
                else
                {
                    return 0;
                }
            }
        }

        [NotMapped]
        public decimal TotalSV
        {
            get
            {
                if (this.Product != null)
                {
                    return this.Quantity * this.Product.SV;
                }
                else
                {
                    return 0;
                }
            }
        }

        [Display(Name = "الوزن")]
        [NotMapped]
        public int TotalWeight
        {
            get
            {
                if (this.Product != null)
                {
                    return this.Quantity * this.Product.Weight;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int ShippingAddressId { get; set; }

        public Decimal ShippingFee { get; set; }

        public Decimal Tax { get; set; }

        public Decimal Total
        {
            get
            {
                if (this.Items != null && this.Items.Count > 0)
                {
                    decimal total = 0;
                    foreach (var item in this.Items)
                    {
                        total += item.SubTotal;
                    }
                    return total;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Decimal Net
        {
            get
            {
                return Total;
            }
        }
    }
}
