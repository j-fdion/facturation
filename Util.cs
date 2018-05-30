using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using vueAppFactu.Models;

namespace vueAppFactu.Utils
{
    public static class Util
    {
        public static void Paginate<T>(Pagination pagination, ref System.Linq.IQueryable<T> data)
        {
            if (data.Any())
            {
                if (pagination.RowsPerPage > 0)
                    data = (System.Linq.IQueryable<T>)data.OrderBy(pagination.SortBy + (pagination.Descending ? " desc" : " asc"))
                        .Skip((pagination.Page - 1) * pagination.RowsPerPage)
                        .Take(pagination.RowsPerPage);
            }
        }
    }
}
