using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class CategoryProduct
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "RequiredFieldError")]
        [Display(Name = "القسم")]
        public string Name { get; set; }

        [Display(Name = "الصورة")]
        [StringLength(200)]
        public string Image { get; set; }

    } 
}
