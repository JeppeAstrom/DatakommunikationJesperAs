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
        // Skapar upp en klient WebSocket
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri("ws://localhost:8080/"), CancellationToken.None);
        Console.WriteLine("WebSocket-klienten är ansluten.");

        while (true)
        {
            // Läser användarens inmatning
            Console.WriteLine("Skriv ditt meddelande (eller skriv 'exit' för att avsluta):");
            string message = Console.ReadLine();

            // Villkor för att avsluta
            if (message == "exit")
            {
                break;
            }

            // Skapar upp ett dataobjekt och serialisera till JSON
            var data = new { Message = message };
            string jsonInput = JsonConvert.SerializeObject(data);

            // Skickar JSON-data till servern
            byte[] sendBuffer = Encoding.UTF8.GetBytes(jsonInput);
            await client.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

            // Tar emot data från servern
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Deserialiserar och visa den mottagna JSON-datan
            string jsonOutput = Encoding.UTF8.GetString(buffer, 0, result.Count);
            dynamic receivedData = JsonConvert.DeserializeObject(jsonOutput);

            Console.WriteLine($"Mottagen servertid: {receivedData.ServerTime}");
        }
    }
}
