using System;
using System.Linq;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IFamilyStatusRepository:IDisposable
    {
        IQueryable <FamilyStatus> GetListFamStatus ();
        void AddFamStatus(FamilyStatus famStat);
        void DeleteFamStatus(int id);
        void UpdateFamStatus(FamilyStatus famStat);
        void Save();
        FamilyStatus GetStatusById(int id);

    }
}
