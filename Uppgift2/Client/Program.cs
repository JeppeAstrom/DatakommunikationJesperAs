using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Initialize UDP client
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.Connect("127.0.0.1", 12345);  // Replace with the receiver's IP and port

            while (true)
            {
                // Get user input
                Console.WriteLine("Write your message (or type 'exit' to quit):");
                string message = Console.ReadLine();

                // Exit condition
                if (message == "exit")
                {
                    break;
                }

                // Create a data object and serialize to JSON
                var data = new
                {
                    Message = message,
                    Timestamp = DateTime.Now.ToString()
                };
                string jsonData = JsonConvert.SerializeObject(data);

                // Convert to bytes and send
                byte[] messageBytes = Encoding.UTF8.GetBytes(jsonData);
                udpClient.Send(messageBytes, messageBytes.Length);

                Console.WriteLine("Message sent.");
            }
        }
    }
}
