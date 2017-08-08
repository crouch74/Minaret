using Akka.Actor;

namespace Minaret.Helpers.Interfaces
{
    public interface IClusterWrapper
    {
        void Leave(Address address);
        void Join(Address address);
        void Down(Address address);
        void SendCurrentClusterState(IActorRef receiver);
        void Subscribe(IActorRef subscriber, params System.Type[] to);
    }
}
