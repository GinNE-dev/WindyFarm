using Microsoft.EntityFrameworkCore;
using NetCoreServer;
using System.Text;
using WindyFarm.Gin.Database.Models;
using WindyFarm.Gin.Network.Handler;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.ServerLog;

namespace WindyFarm.Gin.Network
{
    public class Session : TcpSession, IPlayer
    {
        private readonly bool DebugByte = false;
        private readonly Server _server;
        public string SessionId { get; }
        public readonly byte[] EncryptKey;
        public readonly byte[] EncryptIV;
        private bool KeySentCompleted;
        private readonly WindyFarmDatabaseContext dbContext;
        private IMessageHandler Handler;

        private bool EncryptionReady => KeySentCompleted && EncryptKey != null && EncryptIV != null;
        public Session(Server server) : base(server)
        {
            _server = server;
            dbContext = server.DbContext;
            EncryptKey = EncryptionHelper.GenerateRandomKey(BlockSize.BitSize128);
            EncryptIV = EncryptionHelper.GenerateRandomIV(BlockSize.BitSize128);
            SessionId = Id.ToString();
            Handler = new ConnectionSetupHandler(server, this);
        }
        protected virtual void OnReceived(Message message)
        {
            if (Handler != null)
            {
                message.Execute(Handler);
            }
            else
            {
                GinLogger.Warning("Handler is not exist now, this mean there are some logic errors!");
            }

        }

        public void SendKey()
        {
            if (DebugByte) GinLogger.Debug("SentKey: " + string.Join(", ", EncryptKey));
            if (DebugByte) GinLogger.Debug("SendIV: " + string.Join(", ", EncryptIV));

            SendKeyMessage message = new() { Key = EncryptKey, IV = EncryptIV };
            SendMessage(message);
        }

        public void VerifyKey(byte[]? key, byte[]? iv)
        {
            string extra = string.Empty;
            bool granted;
            if (key == null || iv == null)
            {
                granted = false;
                extra = "Keys not macth!";
            }
            else
            {
                granted = EncryptKey.SequenceEqual(key) && EncryptIV.SequenceEqual(iv);
            }

            ConnectionResultMessage result = new()
            {
                Granted = granted,
                ExtraMessage = extra
            };

            SendMessage(result);
            KeySentCompleted = true;

            if (!granted)
            {
                GinLogger.Info($"Session {Id} forced to disconnect due to invalid key!");
                base.Disconnect();
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            byte[] data = new byte[(int)size];
            Array.Copy(buffer, 0, data, 0, (int)size);
            if (EncryptionReady)
            {
                if (DebugByte) GinLogger.Debug("ReceivedBytes: " + string.Join(", ", data));
                data = EncryptionHelper.Decrypt(data, EncryptKey, EncryptIV);
                if (DebugByte) GinLogger.Debug("DecryptedBytes: " + string.Join(", ", data));
            }

            string messageString = Encoding.UTF8.GetString(data);
            GinLogger.Debug($"Received {messageString} from {SessionId}");
            Message? message = MessagePool.Instance.ParseMessage(messageString);
            if (message == null)
            {
                GinLogger.Warning($"Received corrupted {messageString}");
                return;
            }

            OnReceived(message);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        protected override void OnConnected()
        {
            GinLogger.Info($"Session {SessionId} connected!");
        }

        protected override void OnDisconnecting()
        {
            //AccountManager.Instance.Remove(_account);
        }

        protected override void OnDisconnected()
        {
            GinLogger.Info($"Session {SessionId} disconnected!");
        }

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public bool SendMessageAsync(IMessage message)
        {
            if (message == null) return false;
            string strMessage = message.Endcode();
            GinLogger.Debug($"Sent {strMessage} to {SessionId}");
            return SendAsync(strMessage);
        }

        public long SendMessage(IMessage message)
        {
            return Send(message.Endcode());
        }

        public override long Send(byte[] buffer)
        {
            if (EncryptionReady)
            {
                buffer = EncryptionHelper.Encrypt(buffer, EncryptKey, EncryptIV);
            }

            return base.Send(buffer);
        }

        public override bool SendAsync(byte[] buffer)
        {
            if (DebugByte) GinLogger.Debug("RawBytes: " + string.Join(", ", buffer));
            if (EncryptionReady)
            {
                buffer = EncryptionHelper.Encrypt(buffer, EncryptKey, EncryptIV);
                if (DebugByte) GinLogger.Debug("EncryptedBytes: " + string.Join(", ", buffer));
            }

            return base.SendAsync(buffer);
        }
    }
}
