using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json; // För JSON-serialisering

public class Program
{
    public static void Main()
    {


        Console.WriteLine("Enter your first name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Enter your last name");
        string lastName = Console.ReadLine();

        Person person1 = new Person
        {
            FirstName = firstName,
            LastName = lastName
        };


        var client = new TcpClient("127.0.0.1", 8888);
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);

        // Skapa en JSON request
        var jsonRequest = JsonConvert.SerializeObject(person1);

        writer.WriteLine(jsonRequest);
        writer.Flush();

        // Läs svaret från servern
        string response = reader.ReadLine();
        Console.WriteLine(response);


        Console.ReadLine();


        client.Close();
    }
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
