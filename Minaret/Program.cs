using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Microsoft.Owin.Hosting;
using Minaret.Actors;

namespace Minaret
{
    class Program
    {
        public static ActorSystem ActorSystem;
        public static IActorRef ClusterManagerActor;
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.Load();
            var apiPort = config.GetString("akka.minaret.api-port");
            var systemName = config.GetString("akka.minaret.system");

            ActorSystem = InitializeActorSystem(systemName);
            ConfigureDependencyResolver(ActorSystem);

            ClusterManagerActor = ActorSystem.ActorOf(ActorSystem.DI().Props<ClusterManagerActor>(), "ClusterManager");
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
                ActorSystem.Terminate();
            }
        }

        private static void ConfigureDependencyResolver(ActorSystem system)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ClusterManagerActor>();

            var container = builder.Build();
            new AutoFacDependencyResolver(container, system);
        }
    }
}
