using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkySigma.Identity.Model.Exceptions
{
    public class SqlException : Exception
    {
        public SqlException() : base("An SQL Query has failed to execute.")
        {
            
        }
    }
}
