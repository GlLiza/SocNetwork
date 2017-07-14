using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class SchoolRepository : ISchoolRepository
    {
        private NetworkContext context;

        public SchoolRepository(NetworkContext context)
        {
            this.context = context;
        }

        public void AddSchool(School school)
        {
            context.School.Add(school);
        }

        public void DeleteSchool(School school)
        {
            School sch = context.School.Find(school.Id);

            context.School.Remove(sch); ;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public School GetSchoolById(int id)
        {
            return context.School.Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateSchool(School school)
        {
            School sch = context.School.Find(school.Id);
            context.Entry(sch).CurrentValues.SetValues(school);
        }
    }
}