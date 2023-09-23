using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Initialize UDP client to listen on port 12345
        using (UdpClient udpClient = new UdpClient(12345))
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Waiting for a message...");

            while (true)
            {
                // Receive data
                byte[] receivedBytes = udpClient.Receive(ref remoteEP);
                string receivedData = Encoding.UTF8.GetString(receivedBytes);

                // Deserialize JSON to dynamic object
                var jsonData = JObject.Parse(receivedData);

                // Display received data
                Console.WriteLine($"Received message: {jsonData["Message"]}, Timestamp: {jsonData["Timestamp"]}");

                // Exit condition
                if ((string)jsonData["Message"] == "exit")
                {
                    break;
                }
            }
        }
    }
}
