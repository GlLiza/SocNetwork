using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repository
{
    public class ConversationRepository : RepositoryBase, IConversationRepository
    {
        public ConversationRepository() { }

        public ConversationRepository(NetworkContext cont) : base(cont)
        {
        }

        public void Add(Conversation conver)
        {
            _context.Conversation.Add(conver);
            Save();
        }

        public void Delete(int id)
        {
            Conversation conver = _context.Conversation.Find(id);
            if (conver != null)
                _context.Conversation.Remove(conver);
        }

        public void Update(Conversation convert)
        {
           _context.Entry(convert).State=EntityState.Modified;
            Save();
        }

        //get conversation's ids list, where curent user is member
        public IQueryable<int> GetConversationsIdsByUserId(int id)
        {
            var listId = _context.Participants.Where(s => s.Users_id == id )
                .Select(s => s.Conversation_id);
            return listId;
        }

        //get companions id list from conversation's  for current user
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
            var conversation = _context.Conversation.FirstOrDefault(s => s.Id== conversationId );
            return conversation;
        }

    }
}