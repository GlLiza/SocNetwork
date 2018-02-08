using System;
using System.Linq;
using DAL.IRepository;
using DAL.EF;

namespace DAL.Repository
{
    public class FamilyStatusRepository : IFamilyStatusRepository
    {
        private NetworkContext context;

        public FamilyStatusRepository(NetworkContext con)
        {
            context = con;
        }

        public void AddFamStatus(FamilyStatus famStat)
        {
            context.FamilyStatus.Add(famStat);
        }

        public void DeleteFamStatus(int id)
        {
            FamilyStatus famStat = context.FamilyStatus.Find(id);
            context.FamilyStatus.Remove(famStat);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //private bool disposed = false;

        //protected virtual void Dispose(bool disposed)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposed)
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}



        public IQueryable<FamilyStatus> GetListFamStatus()
        {
           return context.FamilyStatus;
        }

      
        //public void Save()
        //{
        //    context.SaveChanges();
        //}

        //public void UpdateFamStatus(FamilyStatus famStat)
        //{
        //    context.Entry(famStat).State = EntityState.Modified;
        //}

        public FamilyStatus GetStatusById(int id)
        {
            return context.FamilyStatus.Find(id);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateFamStatus(FamilyStatus famStat)
        {
            throw new NotImplementedException();
        }
    }
}