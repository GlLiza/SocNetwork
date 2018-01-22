using network.BLL.EF;
using network.DAL.Enums;
using network.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace network.Views.ViewModels
{
    public class UserViewModel
    {
    }

    public class UsersDetailsViewModel
    {
        public string Id { get; set; }

        public string RequstedId { get; set; }
        public UserDetails UserDetails { get; set; }
        public IEnumerable<Requests> Requests { get; set; }
    }

    public class ShowUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        public byte[] Image { get; set; }

        public AspNetUsers User { get; set; }
    }

    public class CreateUserViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public Gender Gender { get; set; }

        public string Name { get; set; }
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }
        public HttpPostedFileBase Image { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirthday { get; set; }

        public int? SelectedStatus { get; set; }
        [Display(Name = "Family status")]
        public virtual FamilyStatus FamStat { get; set; }
        public virtual IQueryable<FamilyStatus> FamilyStatus { get; set; }

        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }


        public WorkPeriod WorkPeriod { get; set; }
        public string[] MonthList { get; set; }

        [Display(Name = "School name")]
        public string SchoolName { get; set; }
        public int GraduationYear { get; set; }

        public string Country { get; set; }
        public List<string> ListOfCountry { get; set; }

        public string City { get; set; }
        public IEnumerable<City> ListOfCity { get; set; }


        public string Street { get; set; }
        public string State { get; set; }
        public string HomeCity { get; set; }
        public string HomeCountry { get; set; }
        public string HomeStreet { get; set; }
        public string HomeState { get; set; }
    }

    public class NotFriendShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public byte[] Image { get; set; }
    }

    public class PageViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }
       
        public byte[] Image { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirthday { get; set; }

        [Display(Name = "Family status")]
        public virtual FamilyStatus FamStat { get; set; }

        public WorkPlace Company { get; set; }
        public IEnumerable<WorkPlace> ListPlace { get; set; }

        public School School { get; set; }
        public IEnumerable<School> ListSchools { get; set; }

        public Location CurrentLocation { get; set; }
        public IEnumerable<Location> ListCurLoc { get; set; }

        public Location HomeLocation { get; set; }
        public IEnumerable<Location> ListHomLoc { get; set; }

    }

    public class PersonalInfoViewModel
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirthday { get; set; }

        [Display(Name = "Family status")]
        public int? SelectedStatus { get; set; }
        public virtual IQueryable<FamilyStatus> FamilyStatus { get; set; }



    }
}