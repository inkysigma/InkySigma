using Microsoft.Framework.DependencyInjection;
using Npgsql;

namespace InkySigma.Infrastructure.ServiceBuilder
{
    public static class SqlConnectionBuilder
    {
        public static IServiceCollection AddSqlConnectionBuilder(this IServiceCollection collection)
        {
            var connection = new NpgsqlConnection("Host=127.0.0.1;Port=5432;User Id=Anonymous;Password=password;Database=sigma");
            connection.OpenAsync();
            collection.AddTransient<NpgsqlConnection>(provider => connection);
            return collection;
        }
    }
}
