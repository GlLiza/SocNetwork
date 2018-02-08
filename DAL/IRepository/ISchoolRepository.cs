using System;
using System.Linq;
using DAL.EF;

namespace DAL.IRepository
{
    public interface ISchoolRepository:IDisposable
    {
        void AddSchool(School school);
        void DeleteSchool(School school);
        void UpdateSchool(School school);
        void Save();
        //School GetSchoolById(int id);
        IQueryable<School> GetListSchool(int id);
    }
}
