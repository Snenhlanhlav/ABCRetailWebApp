using Azure.Storage.Files.Shares;

namespace ABCRetailWebApp.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            _shareClient = new ShareClient(connectionString, "logfiles");
            _shareClient.CreateIfNotExists();
        }

        public async Task AppendLogAsync(string logText)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var fileClient = directory.GetFileClient("activitylog.txt");

            string existingContent = "";
            if (await fileClient.ExistsAsync())
            {
                var download = await fileClient.DownloadAsync();
                using var reader = new StreamReader(download.Value.Content);
                existingContent = await reader.ReadToEndAsync();
            }

            string newContent = existingContent + $"{DateTime.Now}: {logText}\n";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(newContent);

            await fileClient.CreateAsync(data.Length);
            using var stream = new MemoryStream(data);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, data.Length), stream);
        }

        public async Task<string> ReadLogAsync()
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var fileClient = directory.GetFileClient("activitylog.txt");

            if (!await fileClient.ExistsAsync())
                return "No logs yet.";

            var download = await fileClient.DownloadAsync();
            using var reader = new StreamReader(download.Value.Content);
            return await reader.ReadToEndAsync();
        }
    }
}