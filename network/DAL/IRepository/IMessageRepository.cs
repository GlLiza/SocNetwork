using System;
using System.Linq;
using network.BLL.EF;
using System.Collections.Generic;

namespace network.DAL.IRepository
{
    interface IMessageRepository:IDisposable
    {
        void AddMessage(Messages message);
        void DeleteMessage(int id);
        void UpdateMessage(Messages message);
      
        IQueryable<Messages> GetListMessages();
        List<Messages> GetListMessagesByConversationId(int conversationId);
    }
}
