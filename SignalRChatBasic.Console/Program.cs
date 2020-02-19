using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRChatBasic.ConsoleApp
{
    class Program
    {
        static HubConnection connection;
        static string name;

        public static async Task Main(string[] args)
        {
            name = "maddy console";
            Console.WriteLine("Enter IP:");
            var ip = Console.ReadLine();
            connection = new HubConnectionBuilder()
                .WithUrl(ip + "/chatHub")
                .Build();

            await connection.StartAsync();
            Console.WriteLine("You are connected!");

            // Handle receiving new messages
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            var keepGoing = true;
            do
            {
                var text = Console.ReadLine();
                if (text == "exit")
                {
                    keepGoing = false;
                }
                else
                {
                    await connection.InvokeAsync("SendMessage", name, text);
                }
            }
            while (keepGoing);
        }
    }
}
