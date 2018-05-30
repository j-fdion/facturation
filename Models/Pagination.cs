using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vueAppFactu.Models
{
    public class Pagination
    {
        public string SortBy { get; set; }

        public bool Descending { get; set; }

        public int Page { get; set; }

        public int RowsPerPage { get; set; }

        public int TotalItems { get; set; }

        public string Search { get; set; }

    }

    public class PaginationWithId
    {
        public string SortBy { get; set; }

        public bool Descending { get; set; }

        public int Page { get; set; }

        public int RowsPerPage { get; set; }

        public int TotalItems { get; set; }

        public string Search { get; set; }

        public Guid Id { get; set; }
    }
}
