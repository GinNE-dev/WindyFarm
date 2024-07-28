using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;

namespace WindyFarm.Gin.Game.Mailing
{
    public class Mail
    {
        public readonly MailDat _mailData;
        public Guid MailId => _mailData.MailId;
        public Guid playerOneId => _mailData.PlayerOneId;
        public Guid playerTwoId => _mailData.PlayerTwoId;
        public DateTime LastUpdateAt => _mailData.UpdateAt;
        public ICollection<MailMessage> MailMessages => _mailData.MailMessages;
        public string PlayerOneName => _mailData.PlayerOne.DisplayName;
        public string PlayerTwoName => _mailData.PlayerTwo.DisplayName;
        public string PlayerOneTreat => _mailData.PlayerOneTreat;
        public string PlayerTwoTreat => _mailData.PlayerTwoTreat;

        public Mail(MailDat mailData) 
        {
            _mailData = mailData;
        }

        public void Update()
        {
            _mailData.UpdateAt = DateTime.Now;
        }

        public bool HasParticipant(Guid participantId)
        {
            return _mailData.PlayerOneId.Equals(participantId) || _mailData.PlayerTwoId.Equals(participantId);
        }

        public void AddMessage(MailMessage message)
        {
            _mailData.MailMessages.Add(message);
        }

        public void MarkAsNewFor(Guid participantId)
        {
            if (playerOneId.Equals(participantId))
            {
                _mailData.PlayerOneTreat = "New";
            }
            else if (playerTwoId.Equals(participantId))
            {
                _mailData.PlayerTwoTreat = "New";
            }
            else { }
        }

        public void MarkAsReadFor(Guid participantId)
        {
            if (playerOneId.Equals(participantId))
            {
                _mailData.PlayerOneTreat = "Read";
            }
            else if (playerTwoId.Equals(participantId))
            {
                _mailData.PlayerTwoTreat = "Read";
            }
            else { }
        }

        public void MarkAsDeletedFor(Guid participantId)
        {
            if (playerOneId.Equals(participantId))
            {
                _mailData.PlayerOneTreat = "Deleted";
            }
            else if (playerTwoId.Equals(participantId))
            {
                _mailData.PlayerTwoTreat = "Deleted";
            }
            else { }
        }

        public string GetTreatOf(Guid participantId)
        {
            if (playerOneId.Equals(participantId))
            {
                return _mailData.PlayerOneTreat;
            }
            else if (playerTwoId.Equals(participantId))
            {
                return _mailData.PlayerTwoTreat;
            }
            else 
            {
                return string.Empty;
            }
        }

        public Guid GetAnotherParticipantId(Guid participantId)
        {
            return participantId.Equals(playerOneId) ? playerTwoId : playerOneId;
        }
    }
}
