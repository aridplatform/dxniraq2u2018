using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class BlogSection
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم القسم الرئيسي")]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
