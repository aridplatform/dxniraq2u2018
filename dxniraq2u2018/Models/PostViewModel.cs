using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Post> Posts { get; set; }

        public PostComment PostComment { get; set; }
        public IEnumerable<PostComment> PostComments { get; set; }

        public PostMetric PostMetric { get; set; }
        public IEnumerable<PostMetric> PostMetrics { get; set; }

        public CommentMetric CommentMetric { get; set; }
        public IEnumerable<CommentMetric> CommentMetrics { get; set; }

        public Guid PostId { get; set; }
    }
}
