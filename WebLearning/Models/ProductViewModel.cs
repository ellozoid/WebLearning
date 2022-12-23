using System.Collections.Generic;

namespace WebLearning.Models
{
    public class ProductViewModel
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int OnPage { get; set; }
        public bool HasPrevPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
