using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebLearning.Contexts;

namespace WebLearning.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NorthwindContext context;

        public ProductsController(NorthwindContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var products = context.Products.Include(x => x.Supplier).Include(x => x.Category).Take(5).ToList();
            return View(products);
        }
    }
}
