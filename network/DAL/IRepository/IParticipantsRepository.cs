using network.BLL.EF;
using System;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IParticipantsRepository:IDisposable
    {
        void AddParticipants(Participants participants);
        void DeleteParticipants(int id);
        void UpdateParicipants(Participants participants);
      

        IQueryable<Participants> GetListParticipants();
    }
}
