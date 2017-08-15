using System.Collections.Generic;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class OpenAlbumViewModel
    {
        public int Id { get; set; }

        public List<Images> Photos{ get; set; }
    }
}