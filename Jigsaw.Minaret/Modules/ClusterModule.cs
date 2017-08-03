using Akka.Actor;
using Akka.Cluster;
using Jigsaw.Minaret.Actors.Messages;
using Nancy;

namespace Jigsaw.Minaret.Modules
{
    public class ClusterModule : NancyModule
    {
        public ClusterModule() : base("api/cluster")
        {
            Get["/", true] = async(_, ct) =>
            {
                var res = await Program.ClusterManagerActor
                    .Ask<ClusterEvent.CurrentClusterState>(new GetClusterStatusMessage());
                return Response.AsJson(res);
            };
        }
    }
}
