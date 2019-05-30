using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "الموضوع")]
        public int BlogPostId { get; set; }
        [Display(Name = "الموضوع")]
        public BlogPost BlogPost { get; set; }

        [Display(Name = "العضو")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        [Display(Name = "العضو")]
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "التعليق")]
        [StringLength(2000, ErrorMessage = "اجعل محتوى التعليق عدد أحرف 10 وأكثر عدد 2000", MinimumLength = 10)]
        public string UserComment { get; set; }

        [Display(Name = "تاريخ التعليق")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        [Display(Name = "حالة الظهور")]
        public bool IsVisible { get; set; }

        [Display(Name = "بلغ عنه")]
        public bool IsAbusedComment { get; set; }

    }
}
