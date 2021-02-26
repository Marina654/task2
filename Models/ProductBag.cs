using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyInvites.Models
{
    public class ProductBag
    {
        public IEnumerable<Product> Products { get; set; }
        public CategoryType? SelectedCategory { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalProductCount { get; set; }
    }
}
