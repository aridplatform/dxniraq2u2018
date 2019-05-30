﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class FaqCategory
    {
        public int Id { get; set; }

        [Display(Name = "اسم القسم")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "المستخدم")]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
