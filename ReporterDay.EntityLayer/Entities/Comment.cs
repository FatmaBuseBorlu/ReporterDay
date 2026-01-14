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
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
