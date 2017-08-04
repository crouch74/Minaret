using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jigsaw.Minaret.Actors.Messages
{
    public class JoinClusterMessage : BaseClusterMessage
    {
        public JoinClusterMessage(string address) : base(address)
        {
        }
    }
}
