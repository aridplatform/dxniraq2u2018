using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dxniraq2u2018.Models
{
    public class LectureViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public Lecture Lecture { get; set; }
        public IEnumerable<Lecture> Lectures { get; set; }
    }
}
