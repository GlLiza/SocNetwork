using System;
using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.Repository;
using System.Globalization;
using DAL.EF;

namespace BLL
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public  IEnumerable<UserDetails> GetUser()
        {
            var users = _userRepository.GetUserList();
            return users;
        }

        public void InsertUser(UserDetails user)
        {
            _userRepository.AddUser(user);
        }

        public void EditUser(UserDetails user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(UserDetails user)
        {
            UserDetails us = _userRepository.GetUserById(user.Id);
            _userRepository.DeleteUser(us.Id);
        }

        public UserDetails SearchUser(int? id)
        {
            return _userRepository.GetUserById(id);
        }

        public IQueryable<FamilyStatus> GetFamStatuses()
        {
            return _userRepository.GetFamStatuses();
        }



        //позволяет преобразовать список string-Id в int-Id
        public List<int> ConvertListId(List<string> strList)
        {
            List<int> listInt = new List<int>();

            foreach (var strId in strList)
            {
                var idInt = ConvertId(strId);
                listInt.Add(idInt);
            }
            return listInt;
        }

        public int ConvertId(string id)
        {
            var user = _userRepository.ReturnIntId(id);
            return user;
        }

       
        

        //позволяет преобразовать string-Id в int-Id для метода GetFriendsForSearch()
        public Tuple<int, List<int>> ConvertListIds(string id,List<string> strList )
        {
            int intIduser = ConvertId(id);
            List<int> intListFriends = ConvertListId(strList);
            return Tuple.Create<int, List<int>>(intIduser, intListFriends);
        }
        
        
        //get data for friend list by id
        public List<UserDetails> GetUserDetailsByListId(List<int> listId)
        {
            List<UserDetails> usersList = new List<UserDetails>();

            foreach (var userId in listId)
            {
                var item = SearchUser(userId);
                usersList.Add(item);
            }
            return usersList;
        }



        //позволяет исключить список id из списка id
        public List<int> ExcludeListIdInListId(List<int> fullList, List<int> excludingList)
        {
            var diff = fullList.Where(x => !excludingList.Contains(x));
            var result = fullList.Where(x => diff.Contains(x)).ToList();
            return result;
        }
        

        //позволяет получить данные для объектов, полученные путем исключения одного списка из другого
        public List<UserDetails> GetDataForSearch(List<int> listIdsAll, List<int> listIdsFromConvers)
        {
            var list = ExcludeListIdInListId(listIdsAll, listIdsFromConvers);
            return GetUserDetailsByListId(list);
        }

        
        //позволяет вернуть все данные для друзей 
        //!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<UserDetails> GetUsersByFriendship(IQueryable<Friendship> friendships)
        {
            List<UserDetails> usersList = new List<UserDetails>();

            foreach (var users in friendships)
            {
                var item = SearchByUserId(users.Friend_id);
                usersList.Add(item);
            }
            return usersList;
        }

        //позволяет получить список прочих пользователей(не друзей)
        //!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<UserDetails> AnotherUsers(IQueryable<Friendship> friendships, List<UserDetails> users)
        {
            List<UserDetails> list = new List<UserDetails>();

            List<UserDetails> friendList = GetUsersByFriendship(friendships);

            var userIds = users.Select(u => u.Id);
            var friendsIds = friendList.Select(f => f.Id);

            var diff = userIds.Where(x => !friendsIds.Contains(x));

            var result = users.Where(x => diff.Contains(x.Id)).ToList();

            return result;
        }


        public List<string> GetCountries()
        {
            List<string> nameOfCountry = new List<string>();
            var country = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                //.Select(x => new Country
                //{
                //    Id = new RegionInfo(x.LCID).Name,
                //    Name = new RegionInfo(x.LCID).EnglishName
                //})
                //.GroupBy(c => c.Id)
                //.Select(c => c.First())
                //.OrderBy(x => x.Name);

            foreach (var cn in country)
            {
                nameOfCountry.Add(cn.Name);
            }

            return nameOfCountry;

        }

        //public string GetCountryNameByCode(IEnumerable<Country> country, string code)
        //{
        //    var cntry = country.Where(s => s.Id == code).Select(s=>s.Name).First();
               
        //    return cntry;
        //}

        public string[] GetMonth()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;                
        }


        public UserDetails SearchByUserId(string id)
        {
            var intId = _userRepository.ReturnIntId(id);
            var user = _userRepository.GetUserById(intId);
            return user;
        }

        public int GetIntUserId(string id)
        {
            var user=SearchByUserId(id);
            return user.Id;
        } 
    }
}







