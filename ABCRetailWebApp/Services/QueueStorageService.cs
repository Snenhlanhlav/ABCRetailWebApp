using Azure.Storage.Queues;

namespace ABCRetailWebApp.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            _queueClient = new QueueClient(connectionString, "orderprocessing");
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message) =>
            await _queueClient.SendMessageAsync(message);

        public async Task<List<string>> PeekMessagesAsync()
        {
            var messages = await _queueClient.PeekMessagesAsync(maxMessages: 10);
            return messages.Value.Select(m => m.MessageText).ToList();
        }
    }
}