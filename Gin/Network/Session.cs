﻿using Microsoft.EntityFrameworkCore;
using NetCoreServer;
using System;
using System.Text;
using WindyFarm.Gin.Core;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game;
using WindyFarm.Gin.Game.Maps;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Handler;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.SystemLog;
using WindyFarm.Utils;

namespace WindyFarm.Gin.Network
{
    public class Session : TcpSession
    {
        private readonly bool DebugByte = false;
        private readonly Server _server;
        public string SessionId { get; }
        public readonly byte[] EncryptKey;
        public readonly byte[] EncryptIV;
        private bool KeySentCompleted;
        private readonly WindyFarmDatabaseContext dbContext;
        private IMessageHandler Handler;
        public Account? AccountData { get; private set; }
        private IPlayer? _player;

        public event Action OnDisconnect = delegate { };
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
                GinLogger.Warning("Handler is not exist, this mean there are some logic errors!");
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

            if (granted)
            {
                KeySentCompleted = true;
                Handler = new AccountHandler(_server, this, dbContext);
            }
            else
            {
                GinLogger.Info($"Session {Id} forced to disconnect due to invalid key!");
                base.Disconnect();
            }
        }

        public void StartHandleGame(Player player)
        {
            Handler = new GameHandler(_server, this, player);
        }

        public void Login(string username, string password)
        {
            string hashedPassword = CryptographyHelper.ComputeSha256Hash(password);
            Account? account = dbContext.Accounts.Include(a => a.PlayerDat).FirstOrDefault(a =>
                a.Email.Equals(username) &&
                a.HashedPassword.Equals(hashedPassword));

            LoginResultMessage resultMessage = new();
            if (account == null)
            {
                resultMessage.Result = LoginResult.IncorrectLoginInfo;
                resultMessage.ExtraMessage = TextDictionary.Get("IncorrectLoginInfo");
                SendMessageAsync(resultMessage);
                return;
            }

            if (AccountManager.Instance.Contains(account))
            {
                resultMessage.Result = LoginResult.LoginOnOtherSession;
                resultMessage.ExtraMessage = TextDictionary.Get("AccountUsedByOther");
                SendMessageAsync(resultMessage);
                return;
            }
            AccountData = account;

            PlayerDat? playerData = account.PlayerDat;

            if (playerData == null)
            {
                resultMessage.Result = LoginResult.MissingCharacter;
                resultMessage.ExtraMessage = TextDictionary.Get("CharacterNotFound");
                SendMessageAsync(resultMessage);
                return;
            }
            _player = new Player(dbContext, _server, this, playerData);
            Handler = new GameHandler(_server, this, _player);

            AccountManager.Instance.Add(account);
            GinLogger.Info($"Client {SessionId} signed as {playerData?.DisplayName}");
            resultMessage.Result = LoginResult.Success;
            resultMessage.MapId = _player.MapId;

            SendMessageAsync(resultMessage);
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private byte[] _buffer = new byte[8192]; // Increased buffer size
        private int _bufferOffset = 0;
        private int _messageLength = 0;
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            int bytesRead = (int)size;
            int bufferOffset = (int)offset;

            while (bytesRead > 0)
            {
                if (_messageLength == 0)
                {
                    if (_bufferOffset < 4)
                    {
                        int lengthToRead = Math.Min(4 - _bufferOffset, bytesRead);
                        Array.Copy(buffer, bufferOffset, _buffer, _bufferOffset, lengthToRead);
                        _bufferOffset += lengthToRead;
                        bufferOffset += lengthToRead;
                        bytesRead -= lengthToRead;

                        if (_bufferOffset == 4)
                        {
                            _messageLength = BitConverter.ToInt32(_buffer, 0);
                            _bufferOffset = 0;

                            // If the message length is zero, reset for next message
                            if (_messageLength == 0)
                            {
                                _messageLength = 0;
                            }
                        }
                    }
                }

                if (_messageLength > 0)
                {
                    int lengthToRead = Math.Min(_messageLength - _bufferOffset, bytesRead);
                    Array.Copy(buffer, bufferOffset, _buffer, _bufferOffset, lengthToRead);
                    _bufferOffset += lengthToRead;
                    bufferOffset += lengthToRead;
                    bytesRead -= lengthToRead;

                    if (_bufferOffset == _messageLength)
                    {
                        OnMessageReceived(_buffer, 0, _messageLength);
                        _messageLength = 0;
                        _bufferOffset = 0;
                    }
                }
            }
        }

        private void OnMessageReceived(byte[] buffer, long offset, long size)
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
                GinLogger.Debug($"Received corrupted {messageString}");
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
            OnDisconnect?.Invoke();
            AccountManager.Instance.Remove(AccountData);
            SessionManager.Instance.Remove(this);
            _server.SaveDataAsync();
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
            string strMessage = message.Encode();
            GinLogger.Debug($"Sent {strMessage} to {SessionId}");
            return SendAsync(strMessage);
        }

        public long SendMessage(IMessage message)
        {
            return Send(message.Encode());
        }

        private byte[] PrepareData(byte[] buffer)
        {
            if (DebugByte) GinLogger.Debug("RawBytes: " + string.Join(", ", buffer));

            if (EncryptionReady)
            {
                buffer = EncryptionHelper.Encrypt(buffer, EncryptKey, EncryptIV);
                if (DebugByte) GinLogger.Debug("EncryptedBytes: " + string.Join(", ", buffer));
            }

            byte[] lengthPrefix = BitConverter.GetBytes(buffer.Length);

            byte[] packet = new byte[lengthPrefix.Length + buffer.Length];
            Array.Copy(lengthPrefix, 0, packet, 0, lengthPrefix.Length);
            Array.Copy(buffer, 0, packet, lengthPrefix.Length, buffer.Length);

            return packet;
        }

        public override long Send(byte[] buffer)
        {
            return base.Send(PrepareData(buffer));
        }

        public override bool SendAsync(byte[] buffer)
        {
            return base.SendAsync(PrepareData(buffer));
        }
    }
}
