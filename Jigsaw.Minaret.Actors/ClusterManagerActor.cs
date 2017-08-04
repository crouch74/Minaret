using System;
using Akka.Actor;
using Akka.Cluster;
using Jigsaw.Minaret.Actors.Messages;

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
            Receive<GetClusterStatusMessage>(message => HandleGetClusterStatusMessage());
            Receive<ClusterEvent.CurrentClusterState>(message => HandleCurrentClusterStateEvent(message));
            Receive<LeaveClusterMessage>(message => HandleLeaveClusterMessage(message));
            Receive<DownClusterMessage>(message => HandleDownClusterMessage(message));
            Receive<JoinClusterMessage>(message => HandleUpClusterMessage(message));
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
        private void HandleLeaveClusterMessage(LeaveClusterMessage message)
        {
            _cluster.Leave(message.Address);
        }
        private void HandleUpClusterMessage(JoinClusterMessage message)
        {
            _cluster.Join(message.Address);
        }

        private void HandleDownClusterMessage(DownClusterMessage message)
        {
            _cluster.Down(message.Address);
        }

        private void RefreshClusterState()
        {
            _cluster.SendCurrentClusterState(Self);
        }
    }
}
