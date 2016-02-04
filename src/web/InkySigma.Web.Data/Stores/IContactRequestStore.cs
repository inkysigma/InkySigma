using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;
using InkySigma.Web.Core;

namespace InkySigma.Web.Data.Stores
{
    public interface IContactRequestStore : IDisposable
    {
        DbConnection Connection { get; }
        string Table { get; }
        bool IsDisposed { get; }
        Task<IEnumerable<ContactRequest>> FindContactRequests(string id, CancellationToken cancellationToken);

        Task<IEnumerable<ContactRequest>> FindContactRequestsByTarget(string target,
            CancellationToken cancellationToken);

        Task<QueryResult> AddContactRequest(ContactRequest request, CancellationToken cancellationToken);
        Task<QueryResult> DeleteContactRequest(ContactRequest request, CancellationToken cancellationToken);
        Task<QueryResult> DeleteUser(string id, CancellationToken cancellationToken);
        Task<QueryResult> UpdateContactRequest(ContactRequest request, CancellationToken cancellationToken);
    }
}