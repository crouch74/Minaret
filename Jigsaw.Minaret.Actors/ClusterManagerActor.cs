using System;
using Akka.Actor;
using Akka.Cluster;

namespace Jigsaw.Minaret.Actors
{
    public class ClusterManagerActor : ReceiveActor
    {
        private readonly Cluster _cluster = Cluster.Get(Context.System);
        private ClusterEvent.CurrentClusterState _currentState;
        public ClusterManagerActor()
        {
            _cluster.Subscribe(Self, typeof(ClusterEvent.IClusterDomainEvent));
            RefreshClusterState();
            Become(Receiving);
        }

        public void Receiving()
        {
            Receive<Messages.GetClusterStatusMessage>(message => HandleGetClusterStatusMessage());
            Receive<ClusterEvent.CurrentClusterState>(message => HandleCurrentClusterStateEvent(message));
            ReceiveAny(message => RefreshClusterState());
        }

        private void HandleCurrentClusterStateEvent(ClusterEvent.CurrentClusterState message)
        {
            _currentState = message;
        }

        private void HandleGetClusterStatusMessage()
        {
            RefreshClusterState();
            Sender.Tell(_currentState);
        }

        private void RefreshClusterState()
        {
            _cluster.SendCurrentClusterState(Self);
        }
    }
}
