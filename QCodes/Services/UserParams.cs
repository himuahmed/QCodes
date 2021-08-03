using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Services
{
    public class UserParams
    {
        private const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 15;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public int UserId { get; set; }
    }
}
