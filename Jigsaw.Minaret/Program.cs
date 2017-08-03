using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace Jigsaw.Minaret
{
    class Program
    {
        static void Main(string[] args)
        {
            StartNancyFxHost("1234");

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
            }
        }
    }
}
