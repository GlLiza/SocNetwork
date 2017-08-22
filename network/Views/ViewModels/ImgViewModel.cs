using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class ImgViewModel
    {
        public List<Images> Images { get; set; }

        public IQueryable<Photoalbum> Albums { get; set; }
    }
}