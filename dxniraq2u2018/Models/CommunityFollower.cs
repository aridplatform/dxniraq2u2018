using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class CommunityFollower
    {
        [Key]
        public Guid Id { get; set; }

        public int CommunityId { get; set; }
        public Community Community { get; set; }

        [StringLength(450)]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public bool NotifyMe { get; set; }
        public bool IsAdmin { get; set; }
    }
}
