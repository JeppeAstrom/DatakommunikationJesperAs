using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Initierar upp en UDP-klient för att lyssna på port 12345
        using (UdpClient udpClient = new UdpClient(12345))
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Väntar på ett meddelande...");

            while (true)
            {
                // Tar emot data
                byte[] receivedBytes = udpClient.Receive(ref remoteEP);
                string receivedData = Encoding.UTF8.GetString(receivedBytes);

                // Deserialiserar JSON till dynamiskt objekt
                var jsonData = JObject.Parse(receivedData);

                // Visar mottagen data
                Console.WriteLine($"Mottaget meddelande: {jsonData["Message"]}, Tidsstämpel: {jsonData["Timestamp"]}");

           
                if ((string)jsonData["Message"] == "exit")
                {
                    break;
                }
            }
        }
    }
}
