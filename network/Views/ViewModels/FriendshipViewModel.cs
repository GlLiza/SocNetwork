using network.BLL.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace network.Views.ViewModels
{
    public class FriendshipViewModel
    {
    }

    public class FriendShow
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        //public HttpPostedFileBase Image { get; set; }
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


    //ViewModel for request

    public class NewRequestsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public byte[] Image { get; set; }
        public Requests Requests { get; set; }
    }

    public class SendRequestViewModel
    {
        public string Id { get; set; }
        public Requests Requests { get; set; }
    }
}