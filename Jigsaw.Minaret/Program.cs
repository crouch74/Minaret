using System;
using Akka.Actor;
using Akka.Configuration;
using Microsoft.Owin.Hosting;

namespace Jigsaw.Minaret
{
    class Program
    {
        private static ActorSystem _actorSystem;

        static void Main(string[] args)
        {
            var config = ConfigurationFactory.Load();
            var apiPort = config.GetString("akka.minaret.api-port");
            var systemName = config.GetString("akka.minaret.system");

            _actorSystem = InitializeActorSystem(systemName);
            StartWebApp(apiPort);
        }

        private static ActorSystem InitializeActorSystem(string systemName)
        {
            return ActorSystem.Create(systemName);
        }

        private static void StartWebApp(string port)
        {
            var url = $"http://+:{port}";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine($"Running on {url}");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
                _actorSystem.Terminate();
            }
        }
    }
}
