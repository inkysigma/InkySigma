using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Chat.Networking.Core
{
    public class Client
    {
        public Connection Connection { get; }

        public Client(Connection connection)
        {
            Connection = connection;
        }
    }
}
