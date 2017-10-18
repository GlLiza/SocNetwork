using System.Collections.Generic;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class SchoolService
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolService()
        {
        }

        public SchoolService(SchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public void AddSchool(School sch)
        {
            _schoolRepository.AddSchool(sch);
        }

        public void DeleteSchool(School sch)
        {
            School school = _schoolRepository.GetSchoolById(sch.Id);

            _schoolRepository.DeleteSchool(school);
        }

        public School SearchSchool(int id)
        {
            return _schoolRepository.GetSchoolById(id);
        }

        public void UpdateSchool(School school)
        {
            _schoolRepository.UpdateSchool(school);
        }

        public IEnumerable<School> GetListSchools(int useId)
        {
            return _schoolRepository.GetListSchool(useId);
        }


    }
}