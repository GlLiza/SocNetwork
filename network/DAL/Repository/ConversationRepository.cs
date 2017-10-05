using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ConversationRepository : RepositoryBase, IConversationRepository
    {
        public ConversationRepository(NetworkContext cont) : base(cont)
        {
        }

        public void AddConversations(Conversation conver)
        {
            context.Conversation.Add(conver);
        }

        public void DeleteConversations(int id)
        {
            Conversation conver = context.Conversation.Find(id);
            if (conver != null)
                context.Conversation.Remove(conver);
        }

        public IQueryable<Conversation> GetListConversations()
        {
            return context.Conversation;
        }

        public void UpdateConversations(Conversation convert)
        {
           context.Entry(convert).State=EntityState.Modified;
        }

       

        //get conversation's ids list by creator's id
        public List<int> GetConversationsIdsByCreatorId(int id)
        {
            return context.Conversation.Where(i => i.Creator_id == id).Select(x=>x.Id).ToList();
        }

        //get users_id from conversation's list of id
        public List<int> GetFriendsIdsList(List<int> conversationIds)
        {
            List<int> friendsIdList=new List<int>();

            foreach (int convId in conversationIds)
            {
                var friendId = context.Participants.SingleOrDefault(x => x.Conversation_id == convId);
                friendsIdList.Add(friendId.Users_id);
            }

            return friendsIdList;
        }

        //

        
    }
}