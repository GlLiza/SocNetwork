using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ParticipantsRepository: RepositoryBase,IParticipantsRepository
    {
        public ParticipantsRepository() 
        {
        }


        public ParticipantsRepository(NetworkContext cont):base(cont)
        {
        }


        public void AddParticipants(Participants participants)
        {
            _context.Participants.Add(participants);
            base.Save();
        }

        public void DeleteParticipants(int id)
        {
            Participants participants = _context.Participants.Find(id);

            if (participants != null)
                _context.Participants.Remove(participants);
        }

        public void UpdateParticipants(Participants participants)
        {
            _context.Entry(participants).State=EntityState.Modified;
        }

        public IQueryable<Participants> GetListParticipants()
        {
            return _context.Participants;
        }

        public List<int> GetListFriendsId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Participants> GetParticipantsByUserId(int userId)
        {
            var particip = _context.Participants.Where(s => s.Users_id == userId).ToList();
            return particip;
        }

        public List<Participants> GetParticipantsByConversId(int conversId)
        {
            var particip = _context.Participants.Where(s => s.Conversation_id == conversId).ToList();
            return particip;
        }
    }
}