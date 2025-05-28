using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporterDay.DataAccessLayer.Abstract
{
    public interface IGenericDal <T> where T: class
    {
        void TInsert (T entity);
        void TUpdate (T entity);
        void TDelete (T entity);
        List<T> GetListAll();
        T GetById(int id);

    }
}
