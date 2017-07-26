using System;
using System.Collections.Generic;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface ISchoolRepository:IDisposable
    {
        void AddSchool(School school);
        void DeleteSchool(School school);
        void UpdateSchool(School school);
        void Save();
        School GetSchoolById(int id);
        IEnumerable<School> GetListSchool(int? id);
    }
}
