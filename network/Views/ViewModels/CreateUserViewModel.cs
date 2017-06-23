using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class CreateUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirthday { get; set; }


        //public int FamStatusId { get; set; }
        //public FamilyStatus FamStat { get; set; }
        //public IEnumerable<FamilyStatus> FamStatusList { get; set; }


        public int SelectedStatus { get; set; }

        [Display(Name = "Family status")]
        public virtual FamilyStatus FamStat { get; set; }

        public virtual ICollection<FamilyStatus> FamilyStatus { get; set; }


        public string Country { get; set; }

        public string City { get; set; }

        //[Display(Name = "Image")]
        public int ImageId { get; set; }
        public Images Image { get; set; }


        public string UserId { get; set; }

    }
}