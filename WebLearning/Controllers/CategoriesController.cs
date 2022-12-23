using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebLearning.Contexts;

namespace WebLearning.Controllers
{
    [Route("Categories")]
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext context;

        public CategoriesController(NorthwindContext context)
        {
            this.context = context;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var categories = await context.Categories.ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Image(int? categoryId)
        {
            if (!categoryId.HasValue)
                return NotFound();

            var category = await context.Categories.FindAsync(categoryId);

            if (category == null)
                return NotFound();

            return File(category.Picture, "image/jpg");
        }
    }
}
