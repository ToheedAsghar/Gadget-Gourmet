using Core.Entities;
using Application;
using Microsoft.AspNetCore.Mvc;
using Web.Data;

namespace Gadget_Gourmet.Controllers
{
    public class ProductController : Controller
    {
        protected readonly ProductService _productService;
        private readonly ApplicationDbContext _context;

        public ProductController(ProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewProduct(int id)
        {
            Product prod = await _productService.GetById(id);
            if (prod != null)
            {
                return View("ViewProductDetails", prod);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult ViewProductDetails(Product product)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var products = await _productService.Search(searchString);
            return View("_ProductListPartial", products);
        }
    }
}
