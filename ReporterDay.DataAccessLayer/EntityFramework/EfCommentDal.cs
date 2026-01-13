using ReporterDay.DataAccessLayer.Abstract;
using ReporterDay.DataAccessLayer.Context;
using ReporterDay.DataAccessLayer.Repositories;
using ReporterDay.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.DataAccessLayer.EntityFramework
{
    public class EfCommentDal : GenericRepository<Comment>, ICommentDal
    {
        private readonly ArticleContext _context;
        public EfCommentDal(ArticleContext context) : base(context)
        {
            _context = context;
        }

        public List<Comment> GetCommentsByArticleId(int id)
        {
            var values = _context.Comments.Where(x => x.ArticleId==id);
            return values.ToList();
        }
    }
}
