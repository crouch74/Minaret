using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster;
using Minaret.Actors.Messages;
using Minaret.Modules.Models;
using Nancy;
using Nancy.ModelBinding;

namespace Minaret.Modules
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

            Post["/down"] = _ =>
            {
                var action = this.Bind<ClusterActionModel>();
                Program.ClusterManagerActor.Tell(new DownClusterMessage(action.Address));
                return String.Empty;
            };

            Post["/leave"] = _ =>
            {
                var action = this.Bind<ClusterActionModel>();
                Program.ClusterManagerActor.Tell(new LeaveClusterMessage(action.Address));
                return String.Empty;
            };

            Post["/join"] = _ =>
            {
                var action = this.Bind<ClusterActionModel>();
                Program.ClusterManagerActor.Tell(new JoinClusterMessage(action.Address));
                return String.Empty;
            };
        }

        private static async System.Threading.Tasks.Task<ClusterEvent.CurrentClusterState> GetCurrentClusterState()
        {
            return await Program.ClusterManagerActor
                                .Ask<ClusterEvent.CurrentClusterState>(new GetClusterStatusMessage());
        }
    }
}
