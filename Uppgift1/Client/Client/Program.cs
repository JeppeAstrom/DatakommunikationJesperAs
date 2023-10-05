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
//using System;
//using System.Net.Sockets;
//using System.Text;

//class Client
//{
//    static void Main()
//    {
//        using var client = new TcpClient("172.20.200.200", 12345);
//        using var stream = client.GetStream();

//        var msg = "Hej på dig. Snark Vöcky $&";
//        var msgBytes = Encoding.UTF8.GetBytes(msg);
//        stream.Write(msgBytes, 0, msgBytes.Length);

//        var buffer = new byte[4096];
//        var bytesRead = stream.Read(buffer, 0, buffer.Length);
//        var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

//        Console.WriteLine(response);
//    }
//}