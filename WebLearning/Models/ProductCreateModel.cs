
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Models
{
    public class ProductCreateModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [MinLength(2)]
        [MaxLength(40)]
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public short UnitsOnOrder { get; set; }

        public short ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public int Supplier { get; set; }

        public int Category { get; set; }
    }
}
