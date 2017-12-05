using network.BLL.EF;
using System.Collections.Generic;
using System.Web;

namespace network.Views.ViewModels
{
    public class PhotoViewModel
    {
    }

    public class ChangePhotoViewModel
    {
        public int Id { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }

    public class AddPhotoViewModel
    {
        public int Id { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }


    //ViewModel for albums

    public class AlbumViewModel
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public Images TitleImage { get; set; }
    }

    public class AlbumImgViewModel
    {
        public List<Images> AllImages { get; set; }
    }


    public class ALbumsViewModel
    {
        public List<AlbumViewModel> Albums { get; set; }

        public AlbumImgViewModel Images { get; set; }
    }

    public class OpenAlbumViewModel
    {
        public int Id { get; set; }

        public List<Images> Photos { get; set; }

        public string NameAlb { get; set; }
    }
}