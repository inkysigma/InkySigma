using System.Collections.Generic;

namespace InkySigma.Identity.Repositories.Result
{
    public class QueryResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<QueryError> Errors { get; set; } = null;

        public static QueryResult Success()
        {
            return new QueryResult() { Succeeded = true };
        }

        public static QueryResult Fail(params QueryError[] errors)
        {
            return new QueryResult()
            {
                Succeeded = false,
                Errors = new List<QueryError>(errors)
            };
        }
    }
}
