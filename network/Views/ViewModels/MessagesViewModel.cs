using System;
using System.Collections.Generic;

namespace network.Views.ViewModels
{


    //for Messages/Show message
    public class MessagesViewModel
    {
        public string NameSender { get; set; }
        public string FirstNameSender { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }

        public List<string> Message { get; set; }
    }


    public class SelectReceiver
    {
        public List<ConversationViewModel> FriendsList { get; set; }
    }

    
    public class ConversationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public byte[] Image { get; set; }
        public int ConversationId { get; set; }
    }

    public class MessageBlocks
    {
        public string Message { get; set; }
        public DateTime ? Time { get; set; }       
    }

    public class OpenConversationViewModel
    {
        public int ConversationId { get; set; }
        public List<ConversationViewModel> Members { get; set; }
        public List<MessageBlocks> Messages { get; set; }

    }
}