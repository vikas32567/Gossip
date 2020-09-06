using System;
using Microsoft.Azure.Cosmos.Table;

namespace Gossip.Models
{
    public class ChatMessage : TableEntity
    {
        public int SenderId { get; set; }
        public string GroupName { get; set; }
        public string Message { get; set; }

        // public ChatMessage(int senderId, string groupName, string message)
        // {
        //     var rand = new Random();
        //     PartitionKey = "Gossip Chat";
        //     RowKey = rand.Next(10000, 100000).ToString();
        //     SenderId = senderId;
        //     GroupName = groupName;
        //     Message = message;
        // }
    }
}