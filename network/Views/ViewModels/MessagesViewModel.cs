using System;
using System.Collections.Generic;

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
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public byte[] Image { get; set; }
    }
}