using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.Enums;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class UserRepository : RepositoryBase,IUserRepository
    {
        public UserRepository()
        {
        }

        public UserRepository(NetworkContext cont):base(cont)
        {
        }

        public void AddUser(UserDetails user)
        {
           _context.UserDetails.Add(user);
            base.Save();
        }

        public void DeleteUser(int userId)
        {
            UserDetails user = _context.UserDetails.Find(userId);
            _context.UserDetails.Remove(user);
            base.Save();
        }
        
        public IEnumerable<UserDetails> GetUserList()
        {
            return _context.UserDetails;
        }
        
        public void Update(UserDetails user)
        {
            var us = _context.UserDetails.Find(user.Id);
            _context.Entry(us).CurrentValues.SetValues(user);
            base.Save();
        }
        
        public UserDetails GetUserById(int? id)
        {
            var item = _context.UserDetails.Find(id);
            return item;
        }

        IEnumerable<UserDetails> Users()
        {
            foreach (var a in GetUserList())
            {
                yield return a;
            }
        }

        public int ReturnIntId(string id)
        {
            var user = _context.UserDetails
                .First(s => s.UserId == id);
            return user.Id;
        }
       

        public IQueryable<UserDetails> GetListFemal(Gender gender)
        {
            string gen = Convert.ToString(Gender.Female);
            var list = _context.UserDetails
                .Where(s => s.Gender == gen);
            return list.AsQueryable();
        }

        public IQueryable<UserDetails> GetListMal(Gender gender)
        {
            string gen = Convert.ToString(Gender.Male);
            var list = _context.UserDetails
                .Where(s => s.Gender == gen);
            return list.AsQueryable();
        }

        public IQueryable<FamilyStatus> GetFamStatuses()
        {
            var list = _context.FamilyStatus;
            return list;
        }
    }
}

