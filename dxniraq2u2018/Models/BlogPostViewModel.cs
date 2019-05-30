using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class BlogPostViewModel
    {
        public BlogPost BlogPost { get; set; }
        public IEnumerable<BlogAlbum> BlogAlbum { get; set; }
    }
}
