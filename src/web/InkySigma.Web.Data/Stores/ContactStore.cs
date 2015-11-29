using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InkySigma.Web.Core;

namespace InkySigma.Web.Data.Stores
{
    public class ContactStore : IDisposable
    {
        public DbConnection Connection { get; }
        public string Table { get; }
        public bool IsDisposed { get; private set; }

        public ContactStore(DbConnection connection, string table)
        {
            Connection = connection;
            Table = table;
        }

        private void Handle(CancellationToken cancellationToken)
        {
            
        }

        public async Task<IEnumerable<Contact>> GetContacts(string id, CancellationToken cancellationToken)
        {
            
            var contacts = await Connection.QueryAsync($"SELECT * FROM {Table} WHERE Id=@id", new {id});
            return contacts.Select(p => new Contact
            {
                Id = p.Id,
                Relation = p.Relation,
                Target = SigmaUser.Create(p.Target)
            });
        }

        public void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ContactStore));
            IsDisposed = true;
        }
    }
}
