using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebLearning.Contexts;

namespace WebLearning.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NorthwindContext context;
        private readonly IConfiguration configuration;

        public ProductsController(NorthwindContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            var products = context.Products
                .Include(x => x.Supplier)
                .Include(x => x.Category)
                .AsQueryable();

            var amountOfProductsShown = configuration["AmountOfProductsShown"];

            if (int.TryParse(amountOfProductsShown, out var productCount) && 
                productCount != 0)
            {
                products = products.Take(productCount);
            }
                
            return View(products.ToList());
        }
    }
}
