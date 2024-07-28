using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Mailing
{
    public class MailBox
    {
        private readonly Player _owner;
        private readonly WindyFarmDatabaseContext _dbContext;
        private readonly Dictionary<Guid, Mail> _inboxes;
        public MailBox(Player owner, WindyFarmDatabaseContext dbContext) 
        { 
            _owner = owner;
            _dbContext = dbContext;
            _inboxes = new();
            LoadMailbox();
        }

        private void LoadMailbox()
        {
            var mailData = _dbContext.MailDats
                .Include(m => m.PlayerOne)
                .Include(m => m.PlayerTwo)
                .Include(m => m.MailMessages)
                .Where(m => m.PlayerOneId.Equals(_owner.Id)|| m.PlayerTwoId.Equals(_owner.Id))
                .ToList();

            foreach (var m in mailData)
            {
                _inboxes.Add(m.MailId, new Mail(m));
            }
        }

        public void MarkMailAsDeleted(Guid mailId)
        {
            var mail = _inboxes.GetValueOrDefault(mailId);
            if(mail is not null) mail.MarkAsDeletedFor(_owner.Id);

            SendMailBox();
        }

        public void SendMailBox()
        {
            MailBoxDataMessage mbm = new MailBoxDataMessage();
            var mails = _inboxes.Values.OrderByDescending(m=>m.LastUpdateAt);
            foreach (var mail in mails)
            {
                if(mail.GetTreatOf(_owner.Id).ToLower().Equals("deleted")) continue;
                mbm.MailIds.Add(mail.MailId);
                mbm.ParnerNames.Add(_owner.Id.Equals(mail.playerOneId) ? mail.PlayerTwoName : mail.PlayerOneName);
                mbm.LastSentTimes.Add(mail.LastUpdateAt);
                mbm.IsNews.Add(mail.GetTreatOf(_owner.Id).ToLower().Equals("new"));
                var  lastMessage = mail.MailMessages.OrderByDescending(m=>m.SentAt).FirstOrDefault();
                mbm.LastMessages.Add(lastMessage is not null ? lastMessage.MessageContent : "...");
            }

            _owner.SendMessageAsync(mbm);
        }

        public void AccessMailByPlayerId(Guid playerId)
        {
            GinLogger.Debug("AccessMailByPlayerId");
            var mail = _inboxes.Values.FirstOrDefault(m=>m.HasParticipant(playerId));
            mail = mail is not null ? mail : StartMail(playerId);
            if(mail is null) return;
            mail.MarkAsReadFor(_owner.Id);
            GinLogger.Debug("AccessMailByPlayerId AccessMailByPlayerId");
            SendMailStream(mail);
        }

        public void AccessMail(Guid mailId)
        {
            var mail = _inboxes.GetValueOrDefault(mailId);
            if (mail is null) return;
            mail.MarkAsReadFor(_owner.Id);
            SendMailStream(mail);
        }

        public void SendMailStream(Mail mail)
        {
            MailStreamDataMessage mtm = new();
            var mailMessages = mail.MailMessages.OrderBy(m => m.SentAt).Take(50);
            mtm.MailID = mail.MailId;
            foreach (var mailMessage in mailMessages)
            {
                mtm.MessageText.Add(mailMessage.MessageContent);
                mtm.SentAts.Add(mailMessage.SentAt);
                mtm.SentBySelfs.Add(mailMessage.SenderId.Equals(_owner.Id));
            }

            _owner.SendMessageAsync(mtm);
        }

        public void ReceiveNewMaiMessage(MailMessage m)
        {
            MailTransactionMessage mt = new();
            mt.Action = MailAction.AddMessage;
            mt.MailId = m.MailId;
            mt.TextMessage = m.MessageContent;

            _owner.SendMessageAsync(mt);
        }

        public void AddMailPassive(Mail mail)
        {
            _inboxes.Add(mail.MailId, mail);
        }

        public Mail? StartMail(Guid passivePlayerId)
        {
            var passivePlayerData = _dbContext.PlayerDats.FirstOrDefault(p=>p.Id.Equals(passivePlayerId));
            if (passivePlayerData is null) return null;
            var newMailDat = new MailDat();
            newMailDat.MailId = Guid.NewGuid();
            newMailDat.PlayerOneId = _owner.Id;
            newMailDat.PlayerTwoId = passivePlayerId;
            newMailDat.PlayerOne = _owner._playerData;
            newMailDat.PlayerTwo = passivePlayerData;

            _dbContext.MailDats.Add(newMailDat);
            _dbContext.SaveChanges();

            var newMail = new Mail(newMailDat);

            _inboxes.Add(newMail.MailId, newMail);

            var onlinePlayer = OnlinePlayerManager.Instance.GetPlayer(passivePlayerId);
            if (onlinePlayer is not null) onlinePlayer.MailBox.AddMailPassive(newMail);

            return newMail;
        }

        public void NewMessage(Guid mailId, string messageText)
        {
            var mail = _inboxes.GetValueOrDefault(mailId);
            if (mail is not null)
            {
                GinLogger.Debug("byActiveMailbyActiveMailbyActiveMailbyActiveMail");
                MailMessage mailMessage = new MailMessage();
                mailMessage.Id = Guid.NewGuid();
                mailMessage.MailId = mailId;
                mailMessage.SenderId = _owner.Id;          
                mailMessage.SentAt = DateTime.Now;
                mailMessage.MessageContent = messageText;
                mailMessage.ReceiverId = mail.GetAnotherParticipantId(_owner.Id);
                mail.MarkAsNewFor(mailMessage.ReceiverId);
                mail.MailMessages.Add(mailMessage);

                _dbContext.MailMessages.Add(mailMessage);
                _dbContext.SaveChanges();

                mail.Update();

                var onlinePlayer = OnlinePlayerManager.Instance.GetPlayer(mailMessage.ReceiverId);
                if (onlinePlayer is not null) onlinePlayer.MailBox.ReceiveNewMaiMessage(mailMessage);
                return;
            }

            GinLogger.Debug("Not founddddddddddddddddddddddddddddddddddddddd");
        }
    }
}
