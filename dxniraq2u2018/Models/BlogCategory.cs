using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم القسم الفرعي")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "القسم الرئيس")]
        public int BlogSectionId { get; set; }
        [Display(Name = "القسم الرئيس")]
        public BlogSection BlogSection { get; set; }

        

    }
}
