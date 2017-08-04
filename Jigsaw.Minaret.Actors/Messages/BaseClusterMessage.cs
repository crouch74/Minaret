using System;
using System.Text.RegularExpressions;
using Akka.Actor;

namespace Jigsaw.Minaret.Actors.Messages
{
    public abstract class BaseClusterMessage
    {
        public Address Address { get; private set; }
        protected BaseClusterMessage(string address)
        {
            // address : akka.tcp://JigsawProfilerSystem@localhost:4035
            var regex = new Regex("(.*)\\:\\/\\/(\\w+)@(\\w+):(\\d+)");
            var matches = regex.Match(address).Groups;
            if (matches.Count < 5)
            {
                throw new Exception("Failed to parse address.");
            }
            var protocol = matches[1].Value;
            var system = matches[2].Value;
            var host = matches[3].Value;
            var port = matches[4].Value;
            Address = new Address(protocol, system, host, int.Parse(port));
        }
    }
}
