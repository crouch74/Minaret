using Akka.Actor;
using Akka.Cluster;
using Akka.DI.Core;
using Minaret.Actors;
using Minaret.Actors.Messages;
using Minaret.Helpers.Interfaces;
using Moq;
using Xunit;

namespace Minaret.Tests
{
    public class ClusterManagerActorTests : MinaretTestKit
    {
        [Fact]
        public void Should_Receive_Leave_Cluster_Message()
        {
            var actor = ActorOf(Sys.DI().Props<Actors.ClusterManagerActor>());

            var message = new LeaveClusterMessage("akka.tcp://MinaretSystem@localhost:4035");
            actor.Tell(message);

            var clusterWrapper = Resolve<Mock<IClusterWrapper>>();
            clusterWrapper.Verify(c => c.Leave(message.Address), Times.Once);
        }

        [Fact]
        public void Should_Receive_Down_Cluster_Message()
        {
            var actor = ActorOf(Sys.DI().Props<Actors.ClusterManagerActor>());

            var message = new DownClusterMessage("akka.tcp://MinaretSystem@localhost:4035");
            actor.Tell(message);

            var clusterWrapper = Resolve<Mock<IClusterWrapper>>();
            clusterWrapper.Verify(c => c.Down(message.Address), Times.Once);
        }

        [Fact]
        public void Should_Receive_Join_Cluster_Message()
        {
            var actor = ActorOf(Sys.DI().Props<Actors.ClusterManagerActor>());

            var message = new JoinClusterMessage("akka.tcp://MinaretSystem@localhost:4035");
            actor.Tell(message);

            var clusterWrapper = Resolve<Mock<IClusterWrapper>>();
            clusterWrapper.Verify(c => c.Join(message.Address), Times.Once);
        }

        [Fact]
        public void Should_Receive_Get_Cluster_Status_Message()
        {
            var actor = ActorOfAsTestActorRef<ClusterManagerActor>(Sys.DI().Props<ClusterManagerActor>(), TestActor);

            var message = new GetClusterStatusMessage();
            actor.Ask(message);

            var clusterWrapper = Resolve<Mock<IClusterWrapper>>();

            clusterWrapper.Verify(cw => cw.SendCurrentClusterState(It.IsAny<IActorRef>()), Times.AtLeastOnce);
        }


    }
}
