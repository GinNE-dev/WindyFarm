using System.Collections.Concurrent;
using WindyFarm.Gin.Database.Models;

namespace WindyFarm.Gin.Game
{
    public class AccountManager
    {
        private readonly ConcurrentDictionary<string, Account> Accounts = new();

        private static AccountManager? _instance;

        public static AccountManager Instance
        {
            get
            {
                _instance ??= new AccountManager();
                return _instance;
            }
        }

        public bool Contains(string email) => Accounts.ContainsKey(email);

        public bool Contains(Account account) => account != null && Accounts.ContainsKey(account.Email);

        public bool Add(Account account)
        {
            return account != null && Accounts.TryAdd(account.Email, account);
        }

        public Account? Remove(string email)
        {
            Accounts.TryRemove(email, out Account? account);
            return account;
        }

        public Account? Remove(Account? account)
        {
            if (account == null) return null;

            Accounts.TryRemove(account.Email, out Account? removed);
            return removed;
        }
        public Account? Get(string email)
        {
            Accounts.TryGetValue(email, value: out Account? account);
            return account;
        }
    }
}
