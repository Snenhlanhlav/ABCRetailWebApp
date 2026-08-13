using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ABCRetailWebApp.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            _containerClient = new BlobContainerClient(connectionString, "productimages");
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public List<string> ListImageUrls()
        {
            var urls = new List<string>();
            foreach (var blob in _containerClient.GetBlobs())
            {
                urls.Add(_containerClient.GetBlobClient(blob.Name).Uri.ToString());
            }
            return urls;
        }

        public async Task DeleteImageAsync(string blobName) =>
            await _containerClient.DeleteBlobIfExistsAsync(blobName);
    }
}