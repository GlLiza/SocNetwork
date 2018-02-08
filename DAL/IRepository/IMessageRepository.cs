using System;
using System.Linq;
using System.Collections.Generic;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IMessageRepository:IDisposable
    {
        void Add(Messages message);
        void Delete(int id);
        void Update(Messages message);
        Messages SearchById(int id);
      
        IQueryable<Messages> GetListMessages();
        IQueryable<Messages> GetListMessagesByConversationId(int? conversationId);
        List<int> GetNotReadingMsg(IQueryable<Messages> list);
    }
}
