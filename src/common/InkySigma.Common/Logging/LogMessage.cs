using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Common.Logging
{
    public class LogMessage
    {
        public int Error { get; set; }

        public string Source { get; set; }
        public string Message { get; set; }
        public string ActionTaken { get; set; }
    }
}
