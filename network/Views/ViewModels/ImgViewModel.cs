using System.Collections.Generic;
using network.BLL.EF;

namespace network.Views.ViewModels
{

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
}