namespace WindyFarm.Gin.Network.Protocol
{
    public interface IMessage
    {
        public MessageTag Tag { get; }
        public string Endcode();
        public void Decode(string json);

        public bool Execute(IMessageHandler messageHandler);
    }
}
