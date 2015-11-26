using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace InkySigma.Web.ApplicationBuilders
{
    public static class SqlConnectionBuilder
    {
        public static IServiceCollection AddSqlConnectionBuilder(this IServiceCollection collection,
            string configuration)
        {
            var connection = new NpgsqlConnection(configuration);
            connection.OpenAsync();
            collection.AddTransient<DbConnection>(provider => connection);
            return collection;
        }
    }
}