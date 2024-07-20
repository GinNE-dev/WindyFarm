using System;
using System.Collections.Concurrent;
using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Network.Protocol
{
    public class MessagePool
    {
        public static readonly MessagePool Instance = new();
        private readonly ConcurrentDictionary<MessageTag, Message> Pool;
        private MessagePool() 
        { 
            Pool = new ConcurrentDictionary<MessageTag, Message>();
        }

        public void Init()
        {
            Register(new RequestKeyMessage());
            Register(new SendKeyMessage());
            Register(new PingMessage());
            Register(new PingReplyMessage());
            Register(new ConfirmKeyMessage());
            Register(new ConnectionResultMessage());
            Register(new TextMessage());
            Register(new LoginMessage());
            Register(new LoginResultMessage());
            Register(new RegisterMessage());
            Register(new RegisterResultMessage());
            Register(new CreateCharacterMessage());
            Register(new CreateCharacterResultMessage());
            Register(new RequestPlayerMessage());
            Register(new PlayerDataMessage());
            Register(new PlayerMovementMessage());
            Register(new InventoryRequestMessage());
            Register(new InventoryResponseMessage());
            Register(new InventoryTransactionMessage());
            Register(new FarmlandRequestMessage());
            Register(new FarmlandResponseMessage());
            Register(new FarmlandTransactionMessage());
            Register(new FarmlandTransactionResultMessage());
            Register(new FarmingShopRequestMessage());
            Register(new FarmingShopDataMessage());
            Register(new FarmingShopTransactionMessage());
            Register(new ItemConsumptionMessage());
            Register(new BarnDataRequestMessage()); 
            Register(new BarnDataResponseMessage());
            Register(new BarnTransactionMessage());
            Register(new BarnSpawnAnimalMessage());
            Register(new BarnTransactionResultMessage());
            Register(new EnterMapMessage());
            Register(new EnterMapResultMessage());
            Register(new PlayerJoinMapMessage());
            Register(new PlayerLeaveMapMessage());
            Register(new RequestDummiesInMapMessage());
            Register(new DummyMovementMessage());   
            Register(new TopListRequestMessage());
            Register(new TopListDataMessage());
        }

        public bool Register(Message? message)
        {
            if(message is null) return false;
            return Pool.TryAdd(message.Tag, message);
        }

        public Message? Get(MessageTag tag)
        {
            Pool.TryGetValue(tag, out Message? message);
            return message?.Clone();
        }

        public Message? ParseMessage(string json)
        {
            Dummy? dummy = JsonHelper.ParseObject<Dummy>(json);

            if (dummy is null)
            {
                GinLogger.Warning($"[{GetType()}]: Can't parse Message tag!");
                return null;
            }
            Message? message = Get(dummy.Tag);
            if (message is null)
            {
                GinLogger.Warning($"[{GetType()}]: Message not found in pool, maybe Message with tag '{dummy.Tag}' not registered!");
                return null;
            }

            message.Decode(json);
            return message;
        }
    }
}
