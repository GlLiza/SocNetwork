using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.IRepository
{
    public interface IConversationRepository:IDisposable
    {
        void Add(Conversation convert);
        void Delete(int id);
        void Update(Conversation convert);

        Conversation GetConversationById(int conversationId);
        Conversation GetByCreatorId(int creatorId);

        IQueryable<int> GetConversationsIdsByUserId(int id);
        List<int> GetFriendsIdsList(List<int> conversationsIdsList, int curUserId);
    }
}
