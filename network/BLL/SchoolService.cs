using System.Collections.Generic;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class SchoolService
    {
        NetworkContext db = new NetworkContext();
        private ISchoolRepository schoolRepository;


        public SchoolService()
        {
            schoolRepository=new SchoolRepository(db);
        }

        public void AddSchool(School sch)
        {
            schoolRepository.AddSchool(sch);
            schoolRepository.Save();
        }

        public void DeleteSchool(School sch)
        {
            School school = schoolRepository.GetSchoolById(sch.Id);

            schoolRepository.DeleteSchool(school);
            schoolRepository.Save();
        }

        public School SearchSchool(int id)
        {
            return schoolRepository.GetSchoolById(id);
        }

        public void UpdateSchool(School school)
        {
            schoolRepository.UpdateSchool(school);
            schoolRepository.Save();
        }

        public IEnumerable<School> GetListSchools(int useId)
        {
            return schoolRepository.GetListSchool(useId);
        }


    }
}