using DAL.EF;
using DAL.IRepository;
using DAL.Repository;
using System.Linq;

namespace BLL
{
    public class SchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        
        public SchoolService(SchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public void AddSchool(School sch)
        {
            _schoolRepository.AddSchool(sch);
        }



        //public void DeleteSchool(School sch)
        //{
        //    School school = _schoolRepository.GetSchoolById(sch.Id);

        //    _schoolRepository.DeleteSchool(school);
        //}

        //public School SearchSchool(int id)
        //{
        //    return _schoolRepository.GetSchoolById(id);
        //}

        public void UpdateSchool(School school)
        {
            _schoolRepository.UpdateSchool(school);
        }

        public IQueryable<School> GetListSchoolsForUser(int userId)
        {
            return _schoolRepository.GetListSchool(userId);
        }


    }
}