using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Services
{
    public class BloodRequestsParams
    {
        private const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 20;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
