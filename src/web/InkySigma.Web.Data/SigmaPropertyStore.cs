using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Authentication.Model.Result;
using InkySigma.Authentication.Repositories;
using InkySigma.Web.Core;
using InkySigma.Web.Data.Options;

namespace InkySigma.Web.Data
{
    public class SigmaPropertyStore : IUserPropertyStore<SigmaUser>
    {
        public DbConnection Connection { get; }
        public TableOptions Tables { get; set; }
        public bool IsDisposed { get; private set; }

        public SigmaPropertyStore(DbConnection connection, TableOptions tables = null)
        {
            Connection = connection;
            Tables = tables ?? new TableOptions();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            if (Connection.State == ConnectionState.Open)
                Connection.Dispose();
        }

        private void Handle(CancellationToken token = default(CancellationToken))
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(SigmaPropertyStore));
            token.ThrowIfCancellationRequested();
        }

        public async Task<SigmaUser> GetProperties(SigmaUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var contacts =
                (await
                    Connection.QueryAsync($"SELECT * FROM {Tables.ContactTable} WHERE Id=@Id", new {user.Id}))
                    .Select(
                        p =>
                            new Contact
                            {
                                Id = p.Id,
                                Relation = p.Relation,
                                Target = SigmaUser.Create(p.Target)
                            });
            user.Contacts = contacts.ToList();
            var requests =
                (await
                    Connection.QueryAsync($"SELECT * FROM {Tables.ContactRequestTable} WHERE Target=@Id", new {user.Id}))
                    .Select(p =>
                        new ContactRequest
                        {
                            Message = p.Message,
                            Name = p.Name,
                            Target = SigmaUser.Create(p.Target)
                        });
            user.InboundRequests = requests.ToList();
            return user;
        }

        public async Task<QueryResult> RemoveUser(SigmaUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            await Connection.ExecuteAsync($"DELETE FROM {Tables.ContactTable} WHERE Id=@Id", new {user.Id});
            await Connection.ExecuteAsync($"DELETE FROM {Tables.ContactRequestTable} WHERE Id=@Id", new {user.Id});
            return QueryResult.Success();
        }

        public async Task<QueryResult> UpdateProperties(SigmaUser user, CancellationToken token)
        {
            Handle(token);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            foreach (Contact contact in user.Contacts)
            {
                var table = await Connection.QueryAsync($"SELECT * FROM {Tables.ContactTable} WHERE Id=@Id AND ");
            }
            return QueryResult.Success();
        }

        public Task<QueryResult> AddUser(SigmaUser user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
