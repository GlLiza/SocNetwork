using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ParticipantsRepository: RepositoryBase,IParticipantsRepository
    {
        public ParticipantsRepository(NetworkContext cont)
        {
            context = cont;
        }


        public void AddParticipants(Participants participants)
        {
            context.Participants.Add(participants);
        }

        public void DeleteParticipants(int id)
        {
            Participants participants = context.Participants.Find(id);

            if (participants != null)
                context.Participants.Remove(participants);
        }

        public void UpdateParicipants(Participants participants)
        {
            context.Entry(participants).State=EntityState.Modified;
        }

        public IQueryable<Participants> GetListParticipants()
        {
            return context.Participants;
        }
    }
}