using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class CommunityViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Community Community { get; set; }
        public IEnumerable<Community> Communities { get; set; }
        public Post Post { get; set; }
        public List<CommunityFollower> CommunityFollower { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<PostComment> PostComments { get; set; }
        public PostMetric PostMetric { get; set; }
        public IEnumerable<PostMetric> PostMetrics { get; set; }
        public CommentMetric CommentMetric { get; set; }
        public IEnumerable<CommentMetric> CommentMetrics { get; set; }

        [Display(Name = "المتابعون")]
        public int FollowersCount { get; set; }

        [Display(Name = "عدد المواضيع")]
        public int PostsCount { get; set; }
    }
    public class CommunityAutofillModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
