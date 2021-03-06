﻿using System.Linq;
using DAL.IRepository;
using DAL.EF;

namespace DAL.Repository
{
    public class SchoolRepository : RepositoryBase,ISchoolRepository
    {

        public SchoolRepository()
        {
        }

        public SchoolRepository(NetworkContext cont):base(cont)
        {
        }

        public void AddSchool(School school)
        {
            _context.School.Add(school);
            base.Save();
        }

        public void DeleteSchool(School school)
        {
            School sch = _context.School.Find(school.Id);

            _context.School.Remove(sch); ;
            base.Save();
        }

        public IQueryable<School> GetListSchool(int id)
        {
            return _context.School.Where(s => s.UserId == id);
        }

        public School GetSchoolById(int id)
        {
            return _context.School.Find(id);
        }
        
        public void UpdateSchool(School school)
        {
            School sch = _context.School.Find(school.Id);
            _context.Entry(sch).CurrentValues.SetValues(school);
            base.Save();
        }

        //public IEnumerable<School> GetListSchool(int? id)
        //{
        //    UserDetails user = _context.UserDetails.Find(id);

        //    var schoolList = _context.School
        //        .Where(s => s.Id == user.SchoolId);

        //    return schoolList;
        //}


    }
}