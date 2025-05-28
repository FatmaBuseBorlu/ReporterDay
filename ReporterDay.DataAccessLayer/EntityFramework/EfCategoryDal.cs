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
    internal class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(ArticleContext context) : base(context)
        {

        }
    }
}
