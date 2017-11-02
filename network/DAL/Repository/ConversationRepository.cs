using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ConversationRepository : RepositoryBase, IConversationRepository
    {
        public ConversationRepository() { }

        public ConversationRepository(NetworkContext cont) : base(cont)
        {
        }

        public void AddConversations(Conversation conver)
        {
            _context.Conversation.Add(conver);
            base.Save();
        }

        public void DeleteConversations(int id)
        {
            Conversation conver = _context.Conversation.Find(id);
            if (conver != null)
                _context.Conversation.Remove(conver);
        }

        public IQueryable<Conversation> GetListConversations()
        {
            return _context.Conversation;
        }

        public void UpdateConversations(Conversation convert)
        {
           _context.Entry(convert).State=EntityState.Modified;
        }

       

        //get conversation's ids list by creator's id
        public List<int> GetConversationsIdsByCreatorId(int id)
        {
            return _context.Conversation.Where(i => i.Creator_id == id).Select(x=>x.Id).ToList();
        }

        //get users_id from conversation's list of id
        public List<int> GetFriendsIdsList(List<int> conversationIds)
        {
            List<int> friendsIdList=new List<int>();

            foreach (int convId in conversationIds)
            {
                var friendId = _context.Participants.SingleOrDefault(x => x.Conversation_id == convId);
                friendsIdList.Add(friendId.Users_id);
            }

            return friendsIdList;
        }

        //return Conversation by creator's id
        public Conversation GetByCreatorId(int creatorId)
        {
            var conversation = _context.Conversation.FirstOrDefault(s => s.Creator_id == creatorId);
            return conversation;
        } 
    }
}