using System.Collections.Concurrent;
using WindyFarm.Gin.Network;

namespace WindyFarm.Gin.Core
{
    public class SessionManager
    {
        private static SessionManager? _instance;
        public ConcurrentDictionary<string, Session> Sessions { get; private set; }
        private SessionManager()
        {
            Sessions = new ConcurrentDictionary<string, Session>();
        }

        public static SessionManager Instance
        {
            get
            {
                _instance ??= new SessionManager();
                return _instance;
            }
        }

        public bool Add(Session user)
        {
            if (user == null) return false;
            return Sessions.TryAdd(user.SessionId, user);
        }

        public Session? Remove(Session user)
        {
            if (user == null) return  null;
            Sessions.TryRemove(user.SessionId, out Session? removed);
            return removed;
        }

        public Session? Remove(string id)
        {
            Sessions.TryRemove(id, out Session? removed);
            return removed;
        }
    }
}
