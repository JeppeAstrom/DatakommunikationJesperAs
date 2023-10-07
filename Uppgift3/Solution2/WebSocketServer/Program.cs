using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        // Initierar upp en HTTP-lyssnare på port 8080
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Start();

        Console.WriteLine("WebSocket-server startad. Lyssnar...");

        while (true)
        {
            // Väntar på en inkommande HTTP-anslutning
            HttpListenerContext context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                // Godkänner WebSocket-anslutning
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                WebSocket webSocket = webSocketContext.WebSocket;

                await EchoLoop(webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private static async Task EchoLoop(WebSocket webSocket)
    {
        // Buffert för att hålla mottagna data
        var buffer = new byte[1024 * 4];
        while (true)
        {
            // Tar emot data
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Deserialiserar JSON-datan
            string jsonInput = Encoding.UTF8.GetString(buffer, 0, result.Count);
            dynamic data = JsonConvert.DeserializeObject(jsonInput);

            // Visar det mottagna meddelande på serverkonsolen
            Console.WriteLine($"Mottaget meddelande från klient: {data.Message}");

            // Skickar tillbaka datan med aktuell tid
            data.ServerTime = DateTime.Now;
            string jsonOutput = JsonConvert.SerializeObject(data);

            // Skicka serialiserad JSON tillbaka till klienten
            byte[] sendBuffer = Encoding.UTF8.GetBytes(jsonOutput);
            await webSocket.SendAsync(new ArraySegment<byte>(sendBuffer), result.MessageType, result.EndOfMessage, CancellationToken.None);
        }
    }
}
