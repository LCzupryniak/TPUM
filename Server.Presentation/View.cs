using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.API;

namespace Server.Presentation
{
    internal class View
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Tworzenie serwisu sklepu...");

            // Utwórz instancję fabryki logiki i serwisu sklepu
            ILogicFactory factory = LogicFactory.CreateFactory();
            IShopService shopService = factory.CreateShopService();

            Console.WriteLine("Inicjalizacja serwera WebSocket...");

            // Utwórz i uruchom WebSocketServer
            var server = new WebSocketServer(8080);
            await server.StartAsync();

            // Program się nie zamyka — StartAsync działa w pętli
        }
    }
}
