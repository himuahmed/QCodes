using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QCodes.Services
{
    public static class Extensions
    {
        public static void Headers(this HttpResponse httpresponse, int totalCount, int totalPage, int currentPage, int itemsPerPage)
        {
            var paginationHeaders = new PaginationHeader(totalCount, totalPage, currentPage, itemsPerPage);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            httpresponse.Headers.Add("paginationHeaders", JsonConvert.SerializeObject(paginationHeaders, camelCaseFormatter));
            httpresponse.Headers.Add("Access-Control-Expose-Headers", "paginationHeaders");
        }
    }
}
