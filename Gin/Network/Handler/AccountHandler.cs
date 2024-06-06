
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindyFarm.Gin.Database.Models;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Account;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.SystemLog;
using WindyFarm.Utils;

namespace WindyFarm.Gin.Network.Handler
{
    public class AccountHandler : MessageHandler
    {
        private readonly WindyFarmDatabaseContext _dbContext;
        private readonly Server _server;
        private readonly Session _session;
        public AccountHandler(Server server, Session session, WindyFarmDatabaseContext dbContext) 
        {
            _server = server;
            _session = session;
            _dbContext = dbContext;
        }

        public override bool handleLogin(LoginMessage message)
        {
            _session.Login(message.Email, message.Password);    
            return true;
        }

        public override bool handleRegister(RegisterMessage message)
        {
            //Validating email format
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            RegisterResultMessage resultMessage = new();
            if (!new Regex(emailPattern).IsMatch(message.Email)) 
            {
                resultMessage.Result = RegisterResult.EmailFormatInvalid;
                resultMessage.ExtraMessage = TextDictionary.Get("EmailFormatInvalid");
                return _session.SendMessageAsync(resultMessage);
            }

            //Checking conformation
            if (!message.Password.Equals(message.Confirmation))
            {
                resultMessage.Result = RegisterResult.ConfirmationPasswordNotMatch;
                resultMessage.ExtraMessage = TextDictionary.Get("ConfirmationPasswordNotMatch");
                return _session.SendMessageAsync(resultMessage);
            }

            //Validating password policy
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$";
            if(!new Regex(passwordPattern).IsMatch(message.Password))
            {
                resultMessage.Result = RegisterResult.ViolationOfPasswordPrivacyPolicy;
                resultMessage.ExtraMessage = TextDictionary.Get("ViolationOfPasswordPrivacyPolicy");
                return _session.SendMessageAsync(resultMessage);
            }

            var account = _dbContext.Accounts.FirstOrDefault(a => a.Email.Equals(message.Email));
            if (account != null)
            {
                resultMessage.Result = RegisterResult.EmailHasBeenUsed;
                resultMessage.ExtraMessage = TextDictionary.Get("EmailHasBeenUsed");
                return _session.SendMessageAsync(resultMessage);
            }

            var newAccount = new Account()
            {
                Email = message.Email,
                HashedPassword = CryptographyHelper.ComputeSha256Hash(message.Password)
            };

            try
            {
                _dbContext.Accounts.Add(newAccount);
                _dbContext.SaveChanges();
                resultMessage.Result = RegisterResult.Success;
            }
            catch (Exception ex)
            {
                GinLogger.Error(ex);
                _dbContext.Accounts.Remove(newAccount);
                resultMessage.Result = RegisterResult.InternalError;
                resultMessage.ExtraMessage = "Some internal error occur on the server, please try again after few seconds!s";
            }

            return _session.SendMessageAsync(resultMessage);
        }
    }
}
