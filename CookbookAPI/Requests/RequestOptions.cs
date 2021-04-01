using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookAPI.Requests
{
    public class RequestOptions
    {
        public string SearchPhrase { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string SortBy { get; set; }

        public string SortDirection { get; set; }
    }
}