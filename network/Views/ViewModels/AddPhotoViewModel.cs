using System.Web;

namespace network.Views.ViewModels
{
    public class AddPhotoViewModel
    {
        public int Id { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}