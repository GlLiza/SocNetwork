using network.BLL.EF;

namespace network.Views.ViewModels
{
    public class SendRequestViewModel
    {
        public string Id { get; set; }
        public Requests Requests { get; set; }
    }
}