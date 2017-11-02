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

        public List<Messages> GetListMessagesByConversationId(int conversationId)
        {
            var listMesg = _context.Messages.Where(s => s.Conversation_id == conversationId).ToList();
            return listMesg;        
        }
    }
}