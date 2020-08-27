
using System;
using System.Threading.Tasks;
using Gossip.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace Gossip.Helpers
{
    public class StorageClient 
    {
        private readonly CloudTable _table;

        public StorageClient(IConfiguration _config)
        {
            var connectionString = _config.GetConnectionString("ChatStorage");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            this._table = tableClient.GetTableReference("ChatMessages");
        }

        public async Task<bool> UploadChatMessage(ChatMessage chatMessage)
        {
            var tableOperation = TableOperation.Insert(chatMessage);

            var result = await _table.ExecuteAsync(tableOperation);
            var insertedMessage = result.Result as ChatMessage;
            Console.WriteLine($"Added user:");

            Console.WriteLine("Status code" + result.HttpStatusCode.ToString());
            Console.WriteLine(result.Result.ToString());
            Console.WriteLine(result.SessionToken?.ToString());
            Console.WriteLine(result.RequestCharge?.ToString());
            Console.WriteLine(result.Etag.ToString());
            Console.WriteLine(result.ActivityId);
            
            if (result.HttpStatusCode == 204)
                return true;

            return false;
        }
        
    }
}