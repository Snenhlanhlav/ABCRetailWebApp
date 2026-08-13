using Microsoft.AspNetCore.Mvc;
using ABCRetailWebApp.Models;
using ABCRetailWebApp.Services;

namespace ABCRetailWebApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableStorageService _tableService;

        public CustomersController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        // GET: /Customers
        public IActionResult Index()
        {
            var customers = _tableService.GetAllCustomers();
            return View(customers);
        }

        // GET: /Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customers/Create
        [HttpPost]
        public async Task<IActionResult> Create(CustomerProfile customer)
        {
            await _tableService.AddCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Customers/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _tableService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}