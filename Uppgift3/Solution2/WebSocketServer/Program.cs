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
        // Initialize HTTP listener on port 8080
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Start();

        Console.WriteLine("WebSocket server started. Listening...");

        while (true)
        {
            // Await incoming HTTP connection
            HttpListenerContext context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                // Accept WebSocket connection
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
        // Buffer to hold received data
        var buffer = new byte[1024 * 4];
        while (true)
        {
            // Receive data
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Deserialize JSON data
            string jsonInput = Encoding.UTF8.GetString(buffer, 0, result.Count);
            dynamic data = JsonConvert.DeserializeObject(jsonInput);

            // Display received message on the server console
            Console.WriteLine($"Received message from client: {data.Message}");

            // Send back the data with current time
            data.ServerTime = DateTime.Now;
            string jsonOutput = JsonConvert.SerializeObject(data);

            // Send serialized JSON back to client
            byte[] sendBuffer = Encoding.UTF8.GetBytes(jsonOutput);
            await webSocket.SendAsync(new ArraySegment<byte>(sendBuffer), result.MessageType, result.EndOfMessage, CancellationToken.None);

        }
    }
}
