using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json; // Används för att serialisera och deserialisera JSON-objekt

public class Program
{
    public static void Main()
    {
        // Startar en TCP-lyssnare som lyssnar på alla IP-adresser på port 8888
        var listener = new TcpListener(IPAddress.Any, 8888);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            // Väntar på en inkommande klientanslutning
            TcpClient client = listener.AcceptTcpClient();

            // När en klient ansluter, hantera dess kommunikation i en separat metod
            HandleClient(client);
        }
    }

    static void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);

        // Läser inkommande data från klienten
        string request = reader.ReadLine();

        // Deserialiserar den inkommande JSON-strängen till ett Person-objekt
        var person = JsonConvert.DeserializeObject<Person>(request);

        // Skapar ett svar baserat på datan som mottagits
        var response = $"Hej, {person.FirstName + " " + person.LastName}!";

        // Sänder svaret tillbaka till klienten
        writer.WriteLine(response);
        writer.Flush();

        // Stänger klientanslutningen
        client.Close();
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
