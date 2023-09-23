using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json; // För JSON-serialisering

public class Program
{
    public static void Main()
    {
        var listener = new TcpListener(IPAddress.Any, 8888);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            // Vänta på en klientanslutning
            TcpClient client = listener.AcceptTcpClient();

            // Hantera klientanslutningen i en separat metod
            HandleClient(client);
        }
    }

    static void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);

        string request = reader.ReadLine();

        // För enkelhets skull, låt oss anta att vi tar emot en JSON-sträng som en request
        var person = JsonConvert.DeserializeObject<Person>(request);
  

        // Svara klienten
        var response = $"Hej, {person.FirstName + " " + person.LastName}!";
        writer.WriteLine(response);
        writer.Flush();

        client.Close();
    }
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
