using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.ApplicationServices;
using network.BLL.EF;

namespace network.Views.ViewModels
{


    //for Messages/Index
    public class MessagesViewModel
    {
        public string NameSender { get; set; }
        public string FirstNameSender { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }
    }


    public class SelectReceiver
    {
       
        public List<FriendMsg> FriendsList { get; set; }

    }

    public class FriendMsg
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public byte[] Image { get; set; }
    }
}