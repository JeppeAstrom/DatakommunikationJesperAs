using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Initierar upp en UDP-klient
        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.Connect("127.0.0.1", 12345);  

            while (true)
            {
                // Tar in användarens inmatning
                Console.WriteLine("Skriv ditt meddelande (eller skriv 'exit' för att avsluta):");
                string message = Console.ReadLine();

                // Villkor för att avsluta
                if (message == "exit")
                {
                    break;
                }

                // Skapar upp ett dataobjekt och serialisera det till JSON
                var data = new
                {
                    Message = message,
                    Timestamp = DateTime.Now.ToString()
                };
                string jsonData = JsonConvert.SerializeObject(data);

                // Konverterar till bytes och skicka
                byte[] messageBytes = Encoding.UTF8.GetBytes(jsonData);
                udpClient.Send(messageBytes, messageBytes.Length);

                Console.WriteLine("Meddelandet skickat.");
            }
        }
    }
}
