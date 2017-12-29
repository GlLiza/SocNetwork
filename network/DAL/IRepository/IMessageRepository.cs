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
        Messages FindMsg(int id);
      
        IQueryable<Messages> GetListMessages();
        IQueryable<Messages> GetListMessagesByConversationId(int? conversationId);
        List<int> GetNotReadingMsg(IQueryable<Messages> list);
    //    IQueryable<Messages> GetNotVisibilityMessage(int conversationId);
    }
}
