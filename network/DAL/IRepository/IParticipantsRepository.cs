using network.BLL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IParticipantsRepository:IDisposable
    {
        void AddParticipants(Participants participants);
        void DeleteParticipants(int id);
        void UpdateParticipants(Participants participants);
        List<int> GetListFriendsId(int id);
        IQueryable<Participants> GetListParticipants();
        List<Participants> GetParticipantsByUserId(int userId);
        List<Participants> GetParticipantsByConversId(int conversId);

    }
}
