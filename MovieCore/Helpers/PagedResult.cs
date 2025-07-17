using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCore.Helpers
{
    internal class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int totalItems { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }


        public PagedResult(List<T> items, int totalitems, int currentpage, int pagesize)
        {
            this.Items = items;
            this.totalItems = totalitems;
            this.currentPage = currentpage;
            this.pageSize = pagesize;

            totalPages = (totalitems + pageSize - 1) / pageSize;

        }
    }
}
