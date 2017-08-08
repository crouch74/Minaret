using Akka.Actor;
using Akka.Cluster;
using Minaret.Helpers.Interfaces;

namespace Minaret.Helpers
{
    public class ClusterWrapper : IClusterWrapper
    {
        private readonly Cluster _cluster;

        public ClusterWrapper(ActorSystem system)
        {
            _cluster = Akka.Cluster.Cluster.Get(system);
        }
        public void Leave(Address address)
        {
            _cluster.Leave(address);
        }

        public void Join(Address address)
        {
            _cluster.Join(address);
        }

        public void Down(Address address)
        {
            _cluster.Down(address);
        }

        public void Subscribe(IActorRef subscriber, params System.Type[] to)
        {
            _cluster.Subscribe(subscriber, to);
        }

        public void SendCurrentClusterState(IActorRef receiver)
        {
            _cluster.SendCurrentClusterState(receiver);
        }
    }
}
