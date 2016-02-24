using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Chat.Networking
{
    public class Client
    {
        public Connection Connection { get; }

        public Client(Connection connection)
        {
            Connection = connection;
        }

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var builder = new StringBuilder();
            builder.Append(message.Command);
            foreach (var parameter in message.Parameters)
                builder.Append($" {parameter}");
            var byteMessage = Encoding.UTF8.GetBytes(builder.ToString());
            await Connection.WriteAsync(byteMessage, cancellationToken);
        }

        public async Task<Message> ReceieveMessageAsync(CancellationToken cancellationToken)
        {
            byte[] receieved = await Connection.ReadAsync(cancellationToken);
            string[] stringMessage = Encoding.UTF8.GetString(receieved).Split(new []{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
        }
    }
}
