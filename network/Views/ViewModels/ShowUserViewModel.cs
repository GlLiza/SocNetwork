using System;
using System.ComponentModel.DataAnnotations;
using network.BLL;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class ShowUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirthday { get; set; }


        //public int FamStatusId { get; set; }
        //public IEnumerable<FamilyStatus> FamStatusList { get; set; }

        [Display(Name = "Family status")]
        public FamilyStatus FamilyStatus { get; set; }


        public string Country { get; set; }

        public string City { get; set; }

        
        //public int ImageId { get; set; }
        //public IEnumerable<Images> ImageList { get; set; }

        public byte[] Image { get; set; }


        public string UserId { get; set; }

        //public ShowUserViewModel()
        //{
            
        //}

        //public ShowUserViewModel(UserDetails a)
        //{
        //    Id = a.Id;
        //    Name = a.Name;
        //    Firstname = a.Firstname;
        //    DateOfBirthday = a.DateOfBirthday;
        //    FamilyStatus = a.;
        //    Country = a.Country;
        //    City = a.City;
        //    Image = a.Images.Data;
        //    UserId = a.UserId;
        //}


    }
}