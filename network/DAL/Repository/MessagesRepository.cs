using System;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using System.Collections.Generic;

namespace network.DAL.Repository
{
    public class MessagesRepository : RepositoryBase, IMessageRepository
    {
        public MessagesRepository()
        {
        }

        public MessagesRepository(NetworkContext cont) : base(cont)
        {
        }

        public void AddMessage(Messages message)
        {
            _context.Messages.Add(message);
            Save();
        }

        public void DeleteMessage(int id)
        {
            Messages msg = _context.Messages.Find(id);
            if (msg != null)
                _context.Messages.Remove(msg);
        }

        public void UpdateMessage(Messages message)
        {
            _context.Entry(message).State = EntityState.Modified;
        }

        public IQueryable<Messages> GetListMessages()
        {
            return _context.Messages;
        }

        public IQueryable<Messages> GetListMessagesByConversationId(int? conversationId)
        {
            var listMesg = _context.Messages
                .Where(s => s.Conversation_id == conversationId);
            return listMesg;        
        }

        public int GetMsg(int conId, int senderId, DateTime date)
        {
            var msg = _context.Messages.FirstOrDefault(x => x.Sender_id == senderId && x.Conversation_id == conId && x.Created_at == date);
            return msg.Id;
        }

        public Messages FindMsg(int id)
        {
            var msgId = _context.Messages.Find(id);
            return msgId;
        }
        
        public List<int> GetNotReadingMsg(IQueryable<Messages> list)
        {
            List<int> result = new List<int>();
            foreach (var item in list)
            {
                var msg = FindMsg(item.Id);
                if (msg.IsNotReading == true)
                    result.Add(msg.Id);
            }
            return result;
        }             
    }
}