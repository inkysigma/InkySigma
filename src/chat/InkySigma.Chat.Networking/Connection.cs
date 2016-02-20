using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace InkySigma.Chat.Networking
{
    public class Connection
    {
        public TcpClient TcpConnection { get; }
        public NetworkStream ConnectionStream { get; }
        public bool IsDisposed { get; private set; }
        public bool IsOpen { get; private set; } = true;
        public int MaxSize { get; }

        public Connection(TcpClient connection, int maxSize = int.MaxValue)
        {
            TcpConnection = connection;
            ConnectionStream = connection.GetStream();
            MaxSize = maxSize;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            ConnectionStream.Dispose();
            TcpConnection.Client.Dispose();
            IsDisposed = true;
        }

        public async Task<byte[]> ReceieveMessageAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (IsDisposed) throw new ObjectDisposedException(nameof(Connection));
            if (!ConnectionStream.CanRead) throw new Common.Exceptions.AccessViolationException(nameof(ConnectionStream));
            int size;
            using (var stream = new MemoryStream())
            {
                var message = new byte[4];
                while (stream.Length < 4 && ConnectionStream.CanRead)
                {
                    int receieved = await ConnectionStream.ReadAsync(message, 0, 4 - Convert.ToInt32(stream.Length), cancellationToken);
                    await stream.WriteAsync(message, 0, receieved, cancellationToken);
                }
                size = BitConverter.ToInt32(stream.ToArray(), 0);
            }
            if (!ConnectionStream.CanRead)
                throw new Common.Exceptions.AccessViolationException(nameof(ConnectionStream));
            if (size > MaxSize)
                return null;
            using (var stream = new MemoryStream())
            {
                var buffer = new byte[512];
                while (stream.Length < size)
                {
                    if (!ConnectionStream.CanRead) throw new Common.Exceptions.AccessViolationException(nameof(ConnectionStream));
                    var receieved = await ConnectionStream.ReadAsync(buffer, 0, size - Convert.ToInt32(stream.Length), cancellationToken);
                    await stream.WriteAsync(buffer, 0, receieved, cancellationToken);
                }
                return stream.ToArray();
            }
        }
        
        public async Task SendMessageAsync(byte[] message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (IsDisposed) throw new ObjectDisposedException(nameof(Connection));
            if (!ConnectionStream.CanRead) throw new Common.Exceptions.AccessViolationException(nameof(ConnectionStream));
            
            var size = message.Length;
            var sizeArray = BitConverter.GetBytes(size);
            if (BitConverter.IsLittleEndian)
                sizeArray = sizeArray.Reverse().ToArray();
            await ConnectionStream.WriteAsync(sizeArray, 0, sizeArray.Length, cancellationToken);
            await ConnectionStream.WriteAsync(message, 0, size, cancellationToken);
        }

        public void Close()
        {
            if (IsDisposed)
                return;
            Dispose();
            IsOpen = false;
        }
    }
}
