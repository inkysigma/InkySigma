using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Chat.Networking
{
    public class Message
    {
        public string Command { get; set; }
        public string Parameters { get; set; }

        public Message(string command, string parameters)
        {
            Command = command;
            Parameters = parameters;
        }

        public void SetParameters(string parameters)
        {
            Parameters = parameters;
        }
    }
}
