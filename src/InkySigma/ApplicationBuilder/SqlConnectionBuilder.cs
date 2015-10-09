using Microsoft.Framework.DependencyInjection;
using Npgsql;

namespace InkySigma.ApplicationBuilder
{
    public static class SqlConnectionBuilder
    {
        public static IServiceCollection AddSqlConnectionBuilder(this IServiceCollection collection,
            string configuration)
        {
            var connection = new NpgsqlConnection(configuration);
            connection.OpenAsync();
            collection.AddTransient(provider => connection);
            return collection;
        }
    }
}