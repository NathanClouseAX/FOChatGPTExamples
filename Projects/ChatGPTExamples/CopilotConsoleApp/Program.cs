namespace CopilotConsoleApp
{
    using System;
    using System.Net.Http;

    //using CopilotWrapper;

    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    //using BotConnectorApp.Service;
    //using BotConnectorApp.Service.Models;
    //using Microsoft.Bot.Connector.DirectLine;
    //using Microsoft.Extensions.Configuration;
    //using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        private static string _watermark = null;
        //private static IBotService _botService;
        //private static AppSettings _appSettings;
        private static string _endConversationMessage;
        private static string _userDisplayName = "You";

        static void Main(string[] args)
        {
            HttpClient _httpClient;

            _httpClient = new HttpClient();


            var response = _httpClient.GetAsync("https://3e90dfd10e85e95bb1d650133250b7.18.environment.api.powerplatform.com/powervirtualagents/botsbyschema/msdyn_AgentCopilot/directline/token?api-version=2022-03-01-preview").Result;

            //DirectLineToken token = JsonConvert.DeserializeObject<DirectLineToken>(response.Content.ReadAsStringAsync().Result);
            
            //Console.Write(token);

            //https://3e90dfd10e85e95bb1d650133250b7.18.environment.api.powerplatform.com/powervirtualagents/botsbyschema/msdyn_AgentCopilot/directline/token?api-version=2022-03-01-preview




            Console.Write("Press  to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                //run loop until Enter is press
            }
        }
    }
}
