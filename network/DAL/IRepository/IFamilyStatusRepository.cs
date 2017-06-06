using System;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IFamilyStatusRepository:IDisposable
    {
        IQueryable <FamilyStatus> GetListFamStatus ();
        void AddFamStatus(FamilyStatus famStat);
        void DeleteFamStatus(string id);
        void UpdateFamStatus(FamilyStatus famStat);
        void Save();
        FamilyStatus GetStatById(string id);

    }
}
