using System.Linq;
using System.Runtime.InteropServices;
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
      Get["/", true] = async (_, ct) =>
        {
          var res = await GetCurrentClusterState();
          return Response.AsJson(res);
        };

      Get["/{role}", true] = async (_, ct) =>
        {
          var state = await GetCurrentClusterState();
          var res = state.Members.Where(m => m.Roles.Contains(_.role));
          return Response.AsJson(res);
        };
    }

    private static async System.Threading.Tasks.Task<ClusterEvent.CurrentClusterState> GetCurrentClusterState()
    {
      return await Program.ClusterManagerActor
                          .Ask<ClusterEvent.CurrentClusterState>(new GetClusterStatusMessage());
    }
  }
}
