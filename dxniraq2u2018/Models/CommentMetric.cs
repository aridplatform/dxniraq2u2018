﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static dxniraq2u2018.Models.Common;

namespace dxniraq2u2018.Models
{
    public class CommentMetric
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PostCommentId { get; set; }
        public virtual PostComment PostComment { get; set; }

        [Display(Name = "المستخدم")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "قيمة التصويت")] // 0 visit  1 like -1 dislike
        public int VoteValue { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "نوع البلاغ")]
        public ReportType ReportType { get; set; }
    }
}
