using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Nancy.Hosting.Self;

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
            StartNancyFxHost(apiPort);
        }

        private static ActorSystem InitializeActorSystem(string systemName)
        {
            return ActorSystem.Create(systemName);
        }

        private static void StartNancyFxHost(string port)
        {
            var config = new HostConfiguration
            {
                UrlReservations = new UrlReservations()
                {
                    CreateAutomatically = true
                }
            };
            using (var host = new NancyHost(config, new Uri($"http://localhost:{port}")))
            {
                host.Start();
                Console.WriteLine($"Running on http://localhost:{port}");
                Console.ReadLine();
                _actorSystem.Terminate();
            }
        }
    }
}
