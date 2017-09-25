using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ConversationRepository : RepositoryBase, IConversationRepository
    {
        public ConversationRepository(NetworkContext cont)
        {
            context = cont;
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
    }
}