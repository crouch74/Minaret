using System;
using Akka.Actor;
using Akka.Cluster;

namespace Jigsaw.Minaret.Actors
{
    public class ClusterManagerActor : ReceiveActor
    {
        private readonly Cluster _cluster = Cluster.Get(Context.System);

        public ClusterManagerActor()
        {
            _cluster
                .Subscribe(Self, typeof(ClusterEvent.IClusterDomainEvent));

            Become(Receiving);
        }

        public void Receiving()
        {
            Receive<ClusterEvent.LeaderChanged>(message => HandleLeaderChanged(message));
        }

        #region Handlers

        private void HandleLeaderChanged(ClusterEvent.LeaderChanged leaderChange)
        {
            
        }
        #endregion
    }
}
