using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using network.Views.ViewModels;
using System;

namespace network.BLL
{
    public class MessagesService
    {
        private readonly IParticipantsRepository _participantsRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImagesRepository _imgRepository;
        private readonly IMessageRepository _msgRepository;

        public MessagesService()
        {
        }

        public MessagesService(ParticipantsRepository participantsRepository, ConversationRepository conversationRepository, 
            FriendshipRepository friendshipRepository, UserRepository userRepository, ImagesRepository imgRepository, MessagesRepository msgRepository)
        {
            _participantsRepository = participantsRepository;
            _conversationRepository = conversationRepository;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _imgRepository = imgRepository ;
            _msgRepository = msgRepository;
        }


        //get all friends
        public IQueryable<Friendship> GetFriendForSelect(string id)
        {
            return _friendshipRepository.GetListFriends(id);
        }

        //get list of friend's Ids without existing conversations
        public List<int> GetFriendsIdsList(int id)
        {
            List<int> friendsIds=new List<int>();

            var conversationIdsList = _conversationRepository.GetConversationsIdsByCreatorId(id);
            if(conversationIdsList!=null)
                friendsIds = _conversationRepository.GetFriendsIdsList(conversationIdsList);
            return friendsIds;
        }

        
        //get  information for friends
        public List<ConversationViewModel> GetUserDetails(List<UserDetails> userList)
        {
            List<ConversationViewModel> detailsList= new List<ConversationViewModel>();

            foreach (var item in userList)
            {
                ConversationViewModel user =new ConversationViewModel();
                var userDetails= _userRepository.GetUserById(item.Id);
                var image = _imgRepository.GetImageById(userDetails.ImagesId);
                
                    user.Id = userDetails.Id;
                    user.FirstName = userDetails.Name;
                    user.LastName = userDetails.Firstname;
                    user.Image = Convert.ToBase64String(image.Data);
                    detailsList.Add(user);
            }
            return detailsList.ToList();
        }
        

        //translate Id-string -> Id-int
        public int GetIntId(string id)
        {
            return _userRepository.ReturnIntId(id);
        }

        

        //  CONVERSATION
        public void CreateConversation(Conversation conversation)
        {
            if (conversation != null)
            {
                _conversationRepository.AddConversations(conversation);
               
            }
        }


        //позволяет получить список id друзей, с которыми существует Conversation
        public List<int> GetFriendsIdListFromConversation(int id)
        {
            List<int> friendsIds=new List<int>();

            var conversationsIds = _conversationRepository.GetConversationsIdsByCreatorId(id);
            if (conversationsIds != null)
            {
               friendsIds = _conversationRepository.GetFriendsIdsList(conversationsIds);
            }
            return friendsIds;
        }

        //return Conversation by creator's id
        public Conversation GetConvByCreatotId(int creatorId)
        {
            var conversation = _conversationRepository.GetByCreatorId(creatorId);
            return conversation;
        }


        //PARTICIPANTS

        public void CreateParticipants(Participants participants)
       {
            if (participants != null)
            {
                _participantsRepository.AddParticipants(participants);
            }
            
        }

        public List<Participants> GetParticipByUsers_id(int Users_id)
        {
            return _participantsRepository.GetParticipantsByUserId(Users_id);
        }

        //public List<EF.Messages> GetMessgByConversationId(int conversationId)
        //{
        //    return _msgRepository.GetListMessagesByConversationId(conversationId);
        //}



        //return list of members by conversation's id 
        public List<ConversationViewModel> GetMembersForParticipants(int? conversationId)
        {
            List<ConversationViewModel> details = new List<ConversationViewModel>();
            var listMembers = _participantsRepository.GetParticipantsByConversId(conversationId);

            foreach (var member in listMembers)
            {
                ConversationViewModel item = new ConversationViewModel();
                var user = _userRepository.GetUserById(member.Users_id);
                item.Id = user.Id;
                item.FirstName = user.Name;
                item.LastName = user.Firstname;
                var photo = _imgRepository.GetImageById(user.ImagesId);
                item.Image = Convert.ToBase64String(photo.Data);

                details.Add(item);
            }
            return details;
        }

        //return messages for conversation
        public List<MessageBlocks> GetMessagesForConversation(int? conversationId)
        {
            List<MessageBlocks> blockMsg = new List<MessageBlocks>();
            var messages = _msgRepository.GetListMessagesByConversationId(conversationId)
                .OrderBy(s => s.Created_at)
                .ToList();
            foreach (var msg in messages)
            {
                MessageBlocks item = new MessageBlocks();
                item.Message = msg.Message;
                item.Time = msg.Created_at;
                item.SenderId = msg.Sender_id;

                var user = _userRepository.GetUserById(msg.Sender_id);
                var img = _imgRepository.GetImageById(user.Id);

                //item.Image =img.Data;
                blockMsg.Add(item);
            }

            return blockMsg;
        }


        public void SendMsg(Messages msg)
        {
            if (msg != null)
            {
                _msgRepository.AddMessage(msg);
            }
        }

    }
}