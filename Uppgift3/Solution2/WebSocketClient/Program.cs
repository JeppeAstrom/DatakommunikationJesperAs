using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a client WebSocket
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri("ws://localhost:8080/"), CancellationToken.None);
        Console.WriteLine("WebSocket client connected.");

        while (true)
        {
            // Read user input
            Console.WriteLine("Write your message (or type 'exit' to quit):");
            string message = Console.ReadLine();

            // Exit condition
            if (message == "exit")
            {
                break;
            }

            // Create data object and serialize to JSON
            var data = new { Message = message };
            string jsonInput = JsonConvert.SerializeObject(data);

            // Send JSON data to server
            byte[] sendBuffer = Encoding.UTF8.GetBytes(jsonInput);
            await client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

            // Receive data from the server
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Deserialize and display received JSON data
            string jsonOutput = Encoding.UTF8.GetString(buffer, 0, result.Count);
            dynamic receivedData = JsonConvert.DeserializeObject(jsonOutput);

            Console.WriteLine($"Received server time: {receivedData.ServerTime}");
        }
    }
}
