using System;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IMessageRepository:IDisposable
    {
        void AddMessage(Messages message);
        void DeleteMessage(int id);
        void UpdateMessage(Messages message);
      
        IQueryable<Messages> GetListMessages();
        IQueryable<Messages> GetListMessagesByConversationId(int? conversationId);
    //    IQueryable<Messages> GetNotVisibilityMessage(int conversationId);
    }
}
