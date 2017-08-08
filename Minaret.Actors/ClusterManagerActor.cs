using System;
using Akka.Actor;
using Akka.Cluster;
using Minaret.Actors.Messages;
using Minaret.Helpers.Interfaces;

namespace Minaret.Actors
{
    public class ClusterManagerActor : ReceiveActor
    {
        private IClusterWrapper _clusterWrapper;
        private ClusterEvent.CurrentClusterState _currentState;
        public ClusterManagerActor(IClusterWrapper clusterWrapper)
        {
            _clusterWrapper = clusterWrapper;
            _clusterWrapper.Subscribe(Self, typeof(ClusterEvent.IClusterDomainEvent));
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
            _clusterWrapper.Leave(message.Address);
        }
        private void HandleUpClusterMessage(JoinClusterMessage message)
        {
            _clusterWrapper.Join(message.Address);
        }

        private void HandleDownClusterMessage(DownClusterMessage message)
        {
            _clusterWrapper.Down(message.Address);
        }

        private void RefreshClusterState()
        {
            _clusterWrapper.SendCurrentClusterState(Self);
        }
    }
}
