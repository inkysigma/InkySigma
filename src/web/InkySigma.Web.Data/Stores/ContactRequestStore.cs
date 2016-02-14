using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Model.Result;
using InkySigma.Web.Core;

namespace InkySigma.Web.Data.Stores
{
    public class ContactRequestStore : IContactRequestStore
    {
        public DbConnection Connection { get; }
        public string Table { get; }
        public bool IsDisposed { get; private set; }

        public ContactRequestStore(DbConnection connection, string table = "user.contactrequests")
        {
            Connection = connection;
            Table = table;
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ContactStore));
            IsDisposed = true;
            if (Connection.State != ConnectionState.Closed)
                Connection.Dispose();
        }

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ContactStore));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<IEnumerable<ContactRequest>> FindContactRequests(string id, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            var contacts = await Connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@Id", new
            {
                Id = id
            });
            return contacts.Select(p => new ContactRequest
            {
                Id = p.Id,
                Name = p.Name,
                UserName = p.Name,
                Message = p.Message,
                Target = SigmaUser.Create(id)
            });
        }

        public async Task<IEnumerable<ContactRequest>> FindContactRequestsByTarget(string target,
            CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(target))
                throw new ArgumentNullException(nameof(target));
            var contacts = await Connection.QueryAsync($"SELECT * FROM {Table} WHERE Target=@Target", new
            {
                Target = target
            });
            return contacts.Select(p => new ContactRequest
            {
                Id = p.Id,
                Name = p.Name,
                UserName = p.Name,
                Message = p.Message,
                Target = SigmaUser.Create(target)
            });
        }

        public async Task<QueryResult> AddContactRequest(ContactRequest request, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Target?.Id))
                throw new InvalidOperationException(nameof(request));
            var count =
                await
                    Connection.ExecuteAsync(
                        $"INSERT INTO {Table} (Id, Message, Name, UserName, Target) VALUES(@Id, @Message, @Name, @UserName, @Target)",
                        new
                        {
                            request.Id,
                            request.Message,
                            request.Name,
                            request.UserName,
                            Target = request.Target.Id
                        });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> DeleteContactRequest(ContactRequest request, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.UserName) 
                || string.IsNullOrEmpty(request.Target?.Id))
                throw new InvalidOperationException(nameof(request));
            var count = await Connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id AND Target=@Target", new
            {
                request.Id,
                Target = request.Target.Id
            });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            var count = await Connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id", new
            {
                Id = id
            });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> UpdateContactRequest(ContactRequest request, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Message) || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Name)
                || string.IsNullOrEmpty(request.Target?.Id))
                throw new InvalidOperationException(nameof(request));
            var count =
                await
                    Connection.ExecuteAsync(
                        $"UPDATE {Table} SET Message=@Message,UserName=@UserName,Name=@Name,Target=@Target WHERE Id=@Id", new
                        {
                            request.Id,

                        });
            return QueryResult.Success(count);
        }
    }
}
