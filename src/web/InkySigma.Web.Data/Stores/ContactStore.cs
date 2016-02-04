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
    public class ContactStore : IContactStore
    {
        public DbConnection Connection { get; }
        public string Table { get; }
        public bool IsDisposed { get; private set; }

        public ContactStore(DbConnection connection, string table = "user.contacts")
        {
            Connection = connection;
            Table = table;
        }

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ContactStore));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task<IEnumerable<Contact>> FindContacts(string id, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            var contacts = await Connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@id", new {id});
            return contacts.Select(p => new Contact
            {
                Id = p.Id,
                Relation = p.Relation,
                Target = SigmaUser.Create(p.Target)
            });
        }

        public async Task<Contact> FindContactByTarget(Contact contact, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Id) || string.IsNullOrEmpty(contact.Target.Id))
                throw new InvalidOperationException(nameof(contact));
            var contacts = await Connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@Id AND Target=@Target", new
            {
                contact.Id,
                Target = contact.Target.Id
            });
            return contacts.Select(p => new Contact
            {
                Id = p.Id,
                Relation = p.Relation,
                Target = SigmaUser.Create(p.Target)
            }).FirstOrDefault();
        }

        public async Task<QueryResult> AddContact(Contact contact, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Relation) || string.IsNullOrEmpty(contact.Id) || string.IsNullOrEmpty(contact.Target.Id))
                throw new InvalidOperationException(nameof(contact));
            var count = await Connection.ExecuteAsync($"INSERT INTO {Table}(Id, Relation, Target) VALUES(@Id, @Relation, @Target)", new
            {
                contact.Id,
                contact.Relation,
                Target = contact.Target.Id
            });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> UpdateContact(Contact contact, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Id) || string.IsNullOrEmpty(contact.Target.Id))
                throw new InvalidOperationException(nameof(contact));
            var count =
                await
                    Connection.ExecuteAsync($"UPDATE {Table} SET Relation=@Relation WHERE Target=@Target AND Id=@Id",
                        new
                        {
                            contact.Id,
                            contact.Relation,
                            Target = contact.Target.Id
                        });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> DeleteContactByTarget(Contact contact, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Id) || string.IsNullOrEmpty(contact.Target.Id))
                throw new InvalidOperationException(nameof(contact));
            var count = await Connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id AND Target=@Target", new
            {
                contact.Id,
                Target = contact.Target.Id
            });
            return QueryResult.Success(count);
        }

        public async Task<QueryResult> DeleteUser(SigmaUser user, CancellationToken cancellationToken)
        {
            Handle(cancellationToken);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id))
                throw new InvalidOperationException(nameof(user));
            var count = await Connection.ExecuteAsync($"DELETE FROM {Table} WHERE Id=@Id", new
            {
                user.Id
            });
            return QueryResult.Success(count);
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ContactStore));
            IsDisposed = true;
            if(Connection.State != ConnectionState.Closed)
                Connection.Dispose();
        }
    }
}
