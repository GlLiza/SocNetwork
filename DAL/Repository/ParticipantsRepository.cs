using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repository
{
    public class ParticipantsRepository: RepositoryBase,IParticipantsRepository
    {
        public ParticipantsRepository() 
        {
        }

        public ParticipantsRepository(NetworkContext cont):base(cont)
        {
        }

        public void Add(Participants participants)
        {
            _context.Participants.Add(participants);
            base.Save();
        }

        public void Delete(int id)
        {
            Participants participants = _context.Participants.Find(id);

            if (participants != null)
                _context.Participants.Remove(participants);
        }

        public void Update(Participants participants)
        {
            _context.Entry(participants).State=EntityState.Modified;
        }

        public List<Participants> GetParticipantsByUserId(int userId)
        {
            var particip = _context.Participants.Where(s => s.Users_id == userId).ToList();
            return particip;
        }

        public List<Participants> GetParticipantsByConversId(int? conversId)
        {
            var particip = _context.Participants.Where(s => s.Conversation_id == conversId).ToList();
            return particip;
        }
    }
}