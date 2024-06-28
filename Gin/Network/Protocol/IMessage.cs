namespace WindyFarm.Gin.Network.Protocol
{
    public interface IMessage
    {
        public MessageTag Tag { get; }
        public string Encode();
        public void Decode(string json);

        public bool Execute(IMessageHandler messageHandler);
    }
}
