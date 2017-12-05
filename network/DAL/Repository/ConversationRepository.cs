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
        public IQueryable<int> GetConversationsIdsByUserId(int id)
        {
            //var db = _context;

            var conversations = from s in _context.Conversation
                                join sa in _context.Participants on s.Id equals sa.Conversation_id
                                where s.Id == sa.Conversation_id
                                select sa.Conversation_id;

            return conversations;

        }

        //get users_id from conversation's list of id
        public List<int> GetFriendsIdsList(List<int> conversationIds, int curUserId)
        {
            List<int> friendsIdList=new List<int>();

            foreach (int convId in conversationIds)
            {
                var participants = _context.Participants.Where(x => x.Conversation_id == convId && x.Users_id!=curUserId);
                foreach (var part in participants)
                {
                    friendsIdList.Add(part.Users_id);
                }
                
            }

            return friendsIdList;
        }

        //return Conversation by creator's id
        public Conversation GetByCreatorId(int creatorId)
        {
            var conversation = _context.Conversation.FirstOrDefault(s => s.Creator_id == creatorId);
            return conversation;
        }

        public Conversation GetConversationById(int conversationId)
        {
            var conversation = _context.Conversation.FirstOrDefault(s => s.Id== conversationId);
            return conversation;
        }

        //public IQueryable<int> GetConversationsIdsByUserId(int id)
        //{
        //    var ids = _context.Conversation.Where(s => s.UserDetails.Id == id).Select(s => s.Id);
        //    return ids;
        //}
    }
}