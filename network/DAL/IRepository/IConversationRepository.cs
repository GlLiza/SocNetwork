using network.BLL.EF;
using System;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IConversationRepository:IDisposable
    {
        void AddConversations(Conversation convert);
        void DeleteConversations(int id);
        void UpdateConversations(Conversation convert);
       
        IQueryable<Conversation> GetListConversations();

    }

  
}
