using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class GalleryImage
    {
        public int Id { get; set; }

        [Display(Name = "العنوان")]
        [StringLength(450)]
        public string Subject { get; set; }

        [Display(Name = "الصورة")]
        [StringLength(450)]
        public string Url { get; set; }

        [Display(Name = "الصورة المصغرة")]
        [StringLength(450)]
        public string _thumb { get; set; }

        [Display(Name = "الالبوم")]
        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }

         }
}
