using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Ajax.Utilities;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class MessagesRepository: RepositoryBase,IMessageRepository
    {
        public MessagesRepository(NetworkContext cont):base(cont)
        {
        }

        public void AddMessage(Messages message)
        {
            context.Messages.Add(message);
            
        }

        public void DeleteMessage(int id)
        {
            Messages msg = context.Messages.Find(id);
            if (msg != null)
                context.Messages.Remove(msg);
        }

        public void UpdateMessage(Messages message)
        {
            context.Entry(message).State=EntityState.Modified;
        }

        public IQueryable<Messages> GetListMessages()
        {
            return context.Messages;
        }

      


    }
}