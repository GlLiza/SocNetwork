using DAL.EF;
using System;
using System.Collections.Generic;

namespace DAL.IRepository
{
    public interface IParticipantsRepository:IDisposable
    {
        void Add(Participants participants);
        void Delete(int id);
        void Update(Participants participants);

        List<Participants> GetParticipantsByUserId(int userId);
        List<Participants> GetParticipantsByConversId(int? conversId);
    }
}
