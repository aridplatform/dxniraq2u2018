using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

       
        [StringLength(450)]
        [Display(Name = "العضو")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "العضو")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "حالة الدفع")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "طريقة الدفع")]
        public int PaymentMethodId { get; set; }
        [Display(Name = "طريقة الدفع")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [StringLength(50)]
        [Display(Name = "الرقم المرجعي لوسيلة الدفع")]
        public string PaymentMethodReference { get; set; }

        [Display(Name = "المحاسب")]
        public string AccountantId { get; set; }
        [Display(Name = "المحاسب")]
        public virtual ApplicationUser Accountant { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ الاصدار")]
        public DateTime IssuingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ الدفع")]
        public DateTime PaidDate { get; set; }

        [Display(Name = "السائق")]
        public string DriverId { get; set; }
        [Display(Name = "السائق")]
        public virtual ApplicationUser Driver { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ التوصيل والدفع")]
        public DateTime DeliveredDate { get; set; }

        [Display(Name = "تم الاتصال به؟")]
        public bool IsCalled { get; set; }

        [Display(Name = "ادخلت الى النظام الرئيسي")]
        public bool IsloggedToDxnSystem { get; set; }

        [StringLength(20)]
        [Display(Name = "الرقم المرجعي من النظام")]
        public string SystemTrackNo { get; set; }

        [Display(Name = "العنوان")]
        public int AddressId { get; set; }
        [Display(Name = "العنوان")]
        public virtual Address Address { get; set; }

        [Display(Name = "الشحن من الفرع")]
        public int BranchId { get; set; }
        [Display(Name = "الشحن من الفرع")]
        public virtual Branch Branch { get; set; }

        [Display(Name = "تقييم عملية الشراء")]
        public Rating CustomerSatisfaction { get; set; }

        [StringLength(300)]
        [Display(Name = "اسم المستلم")]
        public string ReceivedByName { get; set; }

        [StringLength(200)]
        [Display(Name = "رقم نقال المستلم")]
        public string ReceivedByMobile { get; set; }

        [Display(Name = "علاقة المستلم")]
        public ReceivedByRelationship ReceivedByRelationshipId { get; set; }
       

        [StringLength(100)]
        public string InvoicePDF { get; set; }

     
    }
}
