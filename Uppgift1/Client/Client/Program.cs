using System;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json; // Används för att serialisera och deserialisera JSON-objekt

public class Program
{
    public static void Main()
    {
       
        Console.WriteLine("Enter your first name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Enter your last name");
        string lastName = Console.ReadLine();

        // Skapa en ny Person-objekt med användarens inmatning
        Person person1 = new Person
        {
            FirstName = firstName,
            LastName = lastName
        };

        // Koppla upp mot en TCP-server på localhost och port 8888
        var client = new TcpClient("127.0.0.1", 8888);
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);

        // Serialisera Person-objektet till en JSON-sträng
        var jsonRequest = JsonConvert.SerializeObject(person1);

        // Skickar JSON-strängen till servern
        writer.WriteLine(jsonRequest);
        writer.Flush();

        // Tar emot och skriv ut svaret från servern
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
