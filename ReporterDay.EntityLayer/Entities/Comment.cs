using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.EntityLayer.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsValid { get; set; }
    }
}
