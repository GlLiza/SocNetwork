using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class SchoolRepository : RepositoryBase,ISchoolRepository
    {
       
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

        public School GetSchoolById(int id)
        {
            return context.School.Find(id);
        }
        
        public void UpdateSchool(School school)
        {
            School sch = context.School.Find(school.Id);
            context.Entry(sch).CurrentValues.SetValues(school);
        }

        public IEnumerable<School> GetListSchool(int? id)
        {
            UserDetails user = context.UserDetails.Find(id);

            var schoolList = context.School
                .Where(s => s.Id == user.SchoolId);

            return schoolList;
        }


    }
}