using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class InvoiceItem
    {

        [Key]
        public Guid Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Quantity { get; set; }
        
        [NotMapped]
        public decimal Price
        {
            get
            {
                return this.Quantity * this.Product.MemberPrice;
            }
        }

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
                        return this.Product.MemberPrice;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        //[NotMapped]
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


        //[NotMapped]
        //public int TotalPV
        //{
        //    get
        //    {
        //        return this.Quantity * this.Product.PV;
        //    }
        //}

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


        //[NotMapped]
        //public decimal TotalSV
        //{
        //    get
        //    {
        //        return this.Quantity * this.Product.SV;
        //    }
        //}

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

        //[NotMapped]
        //public int TotalWeight
        //{
        //    get
        //    {
        //        return this.Quantity * this.Product.Weight;
        //    }
        //}

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

        [StringLength(20)]
        public string DXNId { get; set; }

        public int InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
