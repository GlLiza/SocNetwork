using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Enums;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IUserRepository:IDisposable
    {
        IEnumerable<UserDetails> GetUserList();
        void AddUser (UserDetails user);
        void DeleteUser (int userId);
        void Update(UserDetails user);
        int ReturnIntId(string id);
        UserDetails GetUserById(int? id);
        IQueryable<UserDetails> GetListFemal(Gender gender);
        IQueryable<UserDetails> GetListMal(Gender gender);

        IQueryable<FamilyStatus> GetFamStatuses();
    }
}
