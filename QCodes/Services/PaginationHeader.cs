using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Services
{
    public class PaginationHeader
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }

        public PaginationHeader(int totalCount, int totalPage, int currentPage, int itemsPerPage)
        {
            TotalCount = totalCount;
            TotalPage = totalPage;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
        }
    }
}
