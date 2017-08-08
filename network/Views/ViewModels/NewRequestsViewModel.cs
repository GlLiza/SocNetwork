using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class NewRequestsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public byte[] Image { get; set; }
        public Requests Requests { get; set; }

    }
}