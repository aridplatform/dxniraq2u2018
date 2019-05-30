using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class BlogAlbum
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "الموضوع")]
        public int BlogPostId { get; set; }
        [Display(Name = "لموضوع")]
        public BlogPost BlogPost { get; set; }

        [Display(Name = "الصورة")]
        [StringLength(450)]
        public string Image { get; set; }

        [Display(Name = "الصورة المصغرة")]
        [StringLength(450)]
        public string ImageThumb { get; set; }

        [Display(Name = "الترتيب")]
        public int OrderId { get; set; }

    }
}
