using System.ComponentModel.DataAnnotations;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class ShowUserViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        public byte[] Image { get; set; }

        public AspNetUsers User { get; set; }
    }
}