using NetCoreServer;
using System.Net;
using WindyFarm.Gin.Core;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Shop;
using WindyFarm.Gin.Network;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin
{
    public class Server : TcpServer
    {
        public static Server? Instance { get; private set; }
        public readonly WindyFarmDatabaseContext DbContext;

        public Server(IPAddress address, int port, WindyFarmDatabaseContext dbContext) : base(address, port) 
        {
            DbContext = dbContext;
            Instance = this;
            ItemReplicator.Init();
            ShopManager.Instance.Init(dbContext);
        }


        public override bool Start()
        {
            GinLogger.Info("Verifying database...");
            if (DbContext == null)
            {
                GinLogger.Warning("Database not found, server cannot start!");
                return false;
            }

            return base.Start();
        }

        protected override void OnStarting()
        {
            base.OnStarting();
            GinLogger.Info($"Server is starting...");
            MessagePool.Instance.Init();
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            GinLogger.Info($"Server is listening on {Address}:{Port}");
        }

        protected override void OnStopping()
        {
            base.OnStopping();
            GinLogger.Info($"Server is stoping...");
            SaveData();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            GinLogger.Info($"Server stopped!");
        }

        public void SaveData()
        {
            GinLogger.Info("Saving data...");
            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                GinLogger.Error(ex);
            }
        }

        public void SaveDataAsync(bool notify = false)
        {
            if(notify) GinLogger.Info("Saving data...");
            try
            {
                DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                GinLogger.Error(ex);
            }
        }

        protected virtual void OnTextMessage(string msg)
        {
            GinLogger.Print(msg);
        }

        protected override TcpSession CreateSession()
        {
            Session userSession = new(this);
            Core.SessionManager.Instance.Add(userSession);
            return userSession;
        }

        public void SendAllText(string text)
        {
            GinLogger.Info($"Sent \"{text}\" to all clients");
            TextMessage message = new() { Text = text };
            foreach (var k_v in Core.SessionManager.Instance.Sessions)
            {
                (k_v.Value as Session).SendMessageAsync(message);
            }
        }
    }
}
