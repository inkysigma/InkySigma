using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InkySigma.Authentication.Model.Result;
using InkySigma.Web.Core;

namespace InkySigma.Web.Data.Stores
{
    public interface IContactStore : IDisposable
    {
        Task<IEnumerable<Contact>> FindContacts(string id, CancellationToken cancellationToken);
        Task<Contact> FindContactByTarget(Contact contact, CancellationToken cancellationToken);
        Task<QueryResult> AddContact(Contact contact, CancellationToken cancellationToken);
        Task<QueryResult> UpdateContact(Contact contact, CancellationToken cancellationToken);
        Task<QueryResult> DeleteContactByTarget(Contact contact, CancellationToken cancellationToken);
        Task<QueryResult> DeleteUser(SigmaUser user, CancellationToken cancellationToken);
    }
}
