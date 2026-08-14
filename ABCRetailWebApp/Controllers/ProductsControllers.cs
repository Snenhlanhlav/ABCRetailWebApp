using Microsoft.AspNetCore.Mvc;
using ABCRetailWebApp.Models;
using ABCRetailWebApp.Services;

namespace ABCRetailWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageService _tableService;
        private readonly BlobStorageService _blobService;

        public ProductsController(TableStorageService tableService, BlobStorageService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        // GET: /Products
        public IActionResult Index()
        {
            var products = _tableService.GetAllProducts();
            return View(products);
        }

        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imageUrl = await _blobService.UploadImageAsync(imageFile);
                product.ImageUrl = imageUrl;
            }

            await _tableService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Products/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _tableService.DeleteProductAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
