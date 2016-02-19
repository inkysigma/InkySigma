using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace InkySigma.Chat.Networking.Core
{
    public class Connection
    {
        public TcpClient TcpConnection { get; }
        public NetworkStream ConnectionStream { get; }
        public bool IsDisposed { get; private set; }
        public bool IsOpen { get; private set; } = true;
        public int MaxSize { get; }

        public Connection(TcpClient connection, int maxSize = Int32.MaxValue)
        {
            TcpConnection = connection;
            ConnectionStream = connection.GetStream();
            MaxSize = maxSize;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            ConnectionStream.Close();
            TcpConnection.Close();
            IsDisposed = true;
        }

        public async Task<byte[]> ReceieveMessageAsync()
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(Connection));
            if (!ConnectionStream.CanRead) throw new AccessViolationException(nameof(ConnectionStream));
            int size;
            using (var stream = new MemoryStream())
            {
                var message = new byte[4];
                while (stream.Length < 4 && ConnectionStream.CanRead)
                {
                    int receieved = await ConnectionStream.ReadAsync(message, 0, 4 - Convert.ToInt32(stream.Length));
                    await stream.WriteAsync(message, 0, receieved);
                }
                size = BitConverter.ToInt32(stream.ToArray(), 0);
            }
            if (!ConnectionStream.CanRead)
                throw new AccessViolationException(nameof(ConnectionStream));
            if (size > MaxSize)
                return null;
            using (var stream = new MemoryStream())
            {
                var buffer = new byte[512];
                while (stream.Length < size)
                {
                    if (!ConnectionStream.CanRead) throw new AccessViolationException(nameof(ConnectionStream));
                    var receieved = await ConnectionStream.ReadAsync(buffer, 0, size - Convert.ToInt32(stream.Length));
                    await stream.WriteAsync(buffer, 0, receieved);
                }
                return stream.ToArray();
            }
        }
        
        public async Task SendMessageAsync(byte[] message)
        {
            if (IsDisposed) throw new ObjectDisposedException(nameof(Connection));
            if (!ConnectionStream.CanRead) throw new AccessViolationException(nameof(ConnectionStream));

            var size = message.Length;
            var sizeArray = BitConverter.GetBytes(size);
            if (BitConverter.IsLittleEndian)
                sizeArray = sizeArray.Reverse().ToArray();
            await ConnectionStream.WriteAsync(sizeArray, 0, sizeArray.Length);
            await ConnectionStream.WriteAsync(message, 0, size);
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
