using System.Collections.Generic;

namespace WebLearning.Models
{
    public class CreateProductViewModel
    {
        public ProductCreateModel ProductModel { get; set; }
        public IEnumerable<KeyValuePair<int, string>> Categories { get; set; }
        public IEnumerable<KeyValuePair<int, string>> Suppliers { get; set; }
    }
}
