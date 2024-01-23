namespace ChatGPTConsole
{
    using ChatGPTWrapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        static void Main(string[] args)
        {
            AssistantGPTClient client = new AssistantGPTClient();

            client.SetBaseURL("https://api.openai.com/v1/threads");
            client.SetPrePrompt("");
            client.SetApiKey("");
            client.SetAssistantId("");

            Console.WriteLine("");
            Console.WriteLine("How do create a customer?");
            Console.WriteLine(client.SendMessageAsync("How do create a customer?"));

            Console.WriteLine("\npress any key to exit the process...");

            // basic use of "Console.ReadKey()" method 
            Console.ReadKey();
        }
    }
}
