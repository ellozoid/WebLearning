using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
            ViewBag.Title = "Products";

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

        [HttpGet]
        public async Task<IActionResult> Add(ProductCreateModel model = null)
        {
            var categories = context.Categories.Select(x => new KeyValuePair<int, string>(x.CategoryId, x.CategoryName));
            var suppliers = context.Suppliers.Select(x => new KeyValuePair<int, string>(x.SupplierId, x.CompanyName));

            ViewBag.Title = "Create new product";

            return View(new CreateProductViewModel()
            {
                ProductModel = model ?? new ProductCreateModel(),
                Categories = await categories.ToListAsync(),
                Suppliers = await suppliers.ToListAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateModel productModel)
        { 
            if (!ModelState.IsValid)
                return RedirectToAction("Add", productModel);

            await context.Products.AddAsync(new Product
            {
                Discontinued = productModel.Discontinued,
                ProductName = productModel.ProductName,
                ReorderLevel = productModel.ReorderLevel,
                QuantityPerUnit = productModel.QuantityPerUnit,
                UnitPrice = productModel.UnitPrice,
                UnitsInStock = productModel.UnitsInStock,
                UnitsOnOrder = productModel.UnitsOnOrder,
                CategoryId = productModel.Category,
                SupplierId = productModel.Supplier
            });
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var product = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id.Value);

            if (product == null) 
                return NotFound();

            var categories = context.Categories.Select(x => new KeyValuePair<int, string>(x.CategoryId, x.CategoryName));
            var suppliers = context.Suppliers.Select(x => new KeyValuePair<int, string>(x.SupplierId, x.CompanyName));

            return View(new CreateProductViewModel
            {
                ProductModel = new ProductCreateModel
                {
                    ProductId = product.ProductId,
                    Category = product.CategoryId,
                    Supplier = product.SupplierId,
                    Discontinued = product.Discontinued,
                    ProductName = product.ProductName,
                    QuantityPerUnit = product.QuantityPerUnit,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder,
                    ReorderLevel = product.ReorderLevel,
                    UnitPrice = product.UnitPrice
                },
                Categories = await categories.ToListAsync(),
                Suppliers = await suppliers.ToListAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateModel productModel)
        {
            var categories = context.Categories.Select(x => new KeyValuePair<int, string>(x.CategoryId, x.CategoryName));
            var suppliers = context.Suppliers.Select(x => new KeyValuePair<int, string>(x.SupplierId, x.CompanyName));

            if (!ModelState.IsValid)
                return View(new CreateProductViewModel
                {
                    ProductModel = productModel,
                    Categories = await categories.ToListAsync(),
                    Suppliers = await suppliers.ToListAsync()
                });

            //context.Attach(new Product
            //{
            //    ProductId = productModel.ProductId,
            //    CategoryId = productModel.Category,
            //    SupplierId = productModel.Supplier,
            //    Discontinued = productModel.Discontinued,
            //    ProductName = productModel.ProductName,
            //    QuantityPerUnit = productModel.QuantityPerUnit,
            //    UnitsInStock = productModel.UnitsInStock,
            //    UnitsOnOrder = productModel.UnitsOnOrder,
            //    ReorderLevel = productModel.ReorderLevel,
            //    UnitPrice = productModel.UnitPrice
            //});

            context.Entry(new Product
            {
                ProductId = productModel.ProductId,
                CategoryId = productModel.Category,
                SupplierId = productModel.Supplier,
                Discontinued = productModel.Discontinued,
                ProductName = productModel.ProductName,
                QuantityPerUnit = productModel.QuantityPerUnit,
                UnitsInStock = productModel.UnitsInStock,
                UnitsOnOrder = productModel.UnitsOnOrder,
                ReorderLevel = productModel.ReorderLevel,
                UnitPrice = productModel.UnitPrice
            }).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
