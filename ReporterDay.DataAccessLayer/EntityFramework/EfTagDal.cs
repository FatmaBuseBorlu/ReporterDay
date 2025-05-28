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
    public class EfTagDal : GenericRepository<Tag>, ItagDal
    {
        public EfTagDal(ArticleContext context) : base(context)
        {
        }
    }
}
