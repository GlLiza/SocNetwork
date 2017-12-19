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


    //for Messages/Index
    public class IndexConversationViewModel
    {
        public int Conversation_id { get; set; }
        public ConversationViewModel Conversation { get; set; }
    }

    public class ConversationViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
    } 

    public class MessageBlocks
    {
        public int SenderId { get; set; }
        public string Message { get; set; }
        public DateTime ? Time { get; set; }       
    }

    //for Messages/OpenConversation
    public class OpenConversationViewModel
    {
        public int? Conversation_id { get; set; }
        public int CurrentUserId { get; set; }
        public List<ConversationViewModel> Members { get; set; }
        public List<MessageBlocks> Messages { get; set; }
    }

    //for Messages/SendMessage
    public class MessageViewModel
    {
        public int Conversation_id { get; set; }
        public string Message { get; set; }
    }
}