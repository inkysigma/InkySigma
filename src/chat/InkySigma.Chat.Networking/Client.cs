namespace InkySigma.Chat.Networking
{
    public class Client
    {
        public Connection Connection { get; }

        public Client(Connection connection)
        {
            Connection = connection;
        }
    }
}
