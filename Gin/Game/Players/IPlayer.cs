namespace WindyFarm.Gin.Game.Players
{
    public interface IPlayer
    {
        public string SessionId { get; }
        public string DisplayName { get; }
        public int Gold { get; }
        public int Diamond { get; }
        public int Level { get; }
        public int Exp { get; }
        public void GainExp(int amount);
    }
}
