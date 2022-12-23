using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebLearning.Contexts;
using WebLearning.Models;

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

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var products = context.Products
                .Include(x => x.Supplier)
                .Include(x => x.Category)
                .AsQueryable();

            var amountOfProductsShown = configuration["AmountOfProductsShown"];

            if (int.TryParse(amountOfProductsShown, out var productCount) && 
                productCount != 0)
            {
                products = products.Skip((pageNumber - 1) * productCount).Take(productCount);
            }

            var productsItems = await products.ToListAsync();
            var totalRecords = await context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)productCount);
            return View(new ProductViewModel()
            {
                Products = productsItems,
                OnPage = productsItems.Count,
                HasNextPage = pageNumber < totalPages,
                HasPrevPage = pageNumber > 1,
                TotalPages = totalPages,
                PageIndex = pageNumber,
                TotalRecords = totalRecords
            });
        }
    }
}
