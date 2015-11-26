using System.Collections.Generic;
using System.Linq;

namespace InkySigma.Authentication.Model.Result
{
    public class QueryResult
    {
        public bool Succeeded { get; set; }
        public int RowsModified { get; set; }
        public List<QueryError> Errors { get; set; }

        public static QueryResult Success()
        {
            return new QueryResult {Succeeded = true};
        }

        public static QueryResult Success(int rowCount)
        {
            return new QueryResult
            {
                Succeeded = true,
                RowsModified = rowCount
            };
        }

        public static QueryResult Fail(params QueryError[] errors)
        {
            return new QueryResult
            {
                Succeeded = false,
                Errors = new List<QueryError>(errors)
            };
        }

        public static QueryResult Fail(string code, string message)
        {
            return new QueryResult
            {
                Succeeded = false,
                Errors = new List<QueryError>
                {
                    new QueryError
                    {
                        Code = code,
                        Description = message
                    }
                }
            };
        }

        public static QueryResult operator +(QueryResult left, QueryResult right)
        {
            if (!left.Succeeded || !right.Succeeded)
                left.Succeeded = false;
            if (left.Errors == null && right.Errors == null)
                return left;
            if (left.Errors == null)
                left.Errors = new List<QueryError>();
            if (right.Errors == null)
                right.Errors = new List<QueryError>();
            left.Errors.AddRange(right.Errors.Where(c => left.Errors.Any(n => n.Description == c.Description)));
            return left;
        }
    }
}