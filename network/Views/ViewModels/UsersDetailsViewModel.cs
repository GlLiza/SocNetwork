using System.Collections.Generic;
using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class UsersDetailsViewModel
    {
        public string Id { get; set; }

        public string RequstedId { get; set; }
        public UserDetails UserDetails { get; set; }
        public IEnumerable<Requests> Requests { get; set; }

    }
}