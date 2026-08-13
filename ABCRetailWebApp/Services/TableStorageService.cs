using Azure.Data.Tables;
using ABCRetailWebApp.Models;

namespace ABCRetailWebApp.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;

        public TableStorageService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];

            _customerTable = new TableClient(connectionString, "CustomerProfiles");
            _customerTable.CreateIfNotExists();

            _productTable = new TableClient(connectionString, "Products");
            _productTable.CreateIfNotExists();
        }

        // Customers
        public async Task AddCustomerAsync(CustomerProfile customer) =>
            await _customerTable.AddEntityAsync(customer);

        public List<CustomerProfile> GetAllCustomers() =>
            _customerTable.Query<CustomerProfile>().ToList();

        public async Task DeleteCustomerAsync(string partitionKey, string rowKey) =>
            await _customerTable.DeleteEntityAsync(partitionKey, rowKey);

        // Products
        public async Task AddProductAsync(Product product) =>
            await _productTable.AddEntityAsync(product);

        public List<Product> GetAllProducts() =>
            _productTable.Query<Product>().ToList();

        public async Task DeleteProductAsync(string partitionKey, string rowKey) =>
            await _productTable.DeleteEntityAsync(partitionKey, rowKey);
    }
}