using Gossip.Helpers;
using Gossip.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gossip.Hubs
{
    public class ChatHub : Hub
    {
        private readonly StorageClient _client;

        public ChatHub(StorageClient _client)
        {
            this._client = _client;
        }

        [HubMethodName("SendMessage")]
        public async Task SendMessageToUser(string groupName, string message)
        {
            var userId = Convert.ToInt32(Context.User.FindFirst(ClaimTypes.Sid).Value);
            var name = Context.User.FindFirst(ClaimTypes.Name).Value;

            Console.WriteLine(name + " says " + message);
            
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", name, message);

            var chatMessage = new ChatMessage();
            var rand = new Random();
            chatMessage.PartitionKey = "Gossip Chat";
            chatMessage.RowKey = rand.Next(10000, 100000).ToString();
            chatMessage.SenderId = userId;
            chatMessage.GroupName = groupName;
            chatMessage.Message = message;

            var result = await _client.UploadChatMessage(chatMessage);

            if (result)
                Console.WriteLine("Successfully sent and saved message.");
            else
                Console.WriteLine("Failed to save message to storage.");
        }

        [HubMethodName("Group")]
        public async Task AddToGroup(string groupName)
        {
            Console.WriteLine($"User {Context.User.FindFirst(ClaimTypes.Name).Value} added to group " +
                $"{groupName}.");
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");

            await LoadChatHistory(groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task LoadChatHistory(string groupName)
        {
            var chatHistory = _client.LoadHistory(groupName);
            var userId = Context.User.FindFirst(ClaimTypes.Sid).Value;
            var name = Context.User.FindFirst(ClaimTypes.Name).Value;

            foreach (var chatMessage in chatHistory)
            {
                
                await Clients
                        .Client(Context.ConnectionId)
                        // .Group(groupName)
                        // .User(userId)
                        .SendAsync("ReceiveMessage", chatMessage.SenderId, chatMessage.Message);
            }
            // foreach (var chatMessage in chatHistory)
            // {
            //     await Clients
            //             .User(name)
            //             .SendAsync("ReceiveMessage", chatMessage.SenderId, chatMessage.Message);
            // }
        }
    }
}