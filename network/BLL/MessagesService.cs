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

        private readonly UserService _userService;       

        public MessagesService(ParticipantsRepository participantsRepository, ConversationRepository conversationRepository, 
            FriendshipRepository friendshipRepository, UserRepository userRepository, ImagesRepository imgRepository, MessagesRepository msgRepository,
            UserService userService)
        {
            _participantsRepository = participantsRepository;
            _conversationRepository = conversationRepository;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _imgRepository = imgRepository ;
            _msgRepository = msgRepository;
            _userService = userService;
        }


        //get all friends
        public IQueryable<Friendship> GetFriendForSelect(string id)
        {           
            return _friendshipRepository.GetListFriends(id);
        }

        //get list of friend's Ids without existing conversations
        //public List<int> GetFriendsIdsList(int id)
        //{
        //    List<int> friendsIds=new List<int>();

        //    var conversationIdsList = _conversationRepository.GetConversationsIdsByUserId(id);
        //    if(conversationIdsList!=null)
        //        friendsIds = _conversationRepository.GetFriendsIdsList(conversationIdsList,id);
        //    return friendsIds;
        //}

        
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


        // data for select receiver 
        public List<UserDetails> GetReceiverForSelect(string id)
        {
            var listIdStr = _friendshipRepository.GetListFriendsId(id);
            var intIds = _userService.ConvertListIds(id, listIdStr);
            var listFrConversation =GetFriendsIdListFromConversation(intIds.Item1);
            var dataOfReceiver = _userService.GetDataForSearch(intIds.Item2, listFrConversation);

            return dataOfReceiver;
        }



        //  CONVERSATION

        //create conversation + create participant
        public Conversation CreateConversation(string id)
        {
            Conversation conversation = new Conversation()
            {
                Creator_id = GetIntId(id),
                Created_at = DateTime.Now.Date
            };
            _conversationRepository.AddConversations(conversation);

            Participants partForCurUser = new Participants
            {
                Conversation_id = conversation.Id,
                Users_id = conversation.Creator_id
            };
            CreateParticipants(partForCurUser);

            return conversation;            
        }


        //позволяет получить список id друзей, с которыми существует Conversation
        public List<int> GetFriendsIdListFromConversation(int id)
        {
            List<int> friendsIds = new List<int>();

            var conversationsIds = _conversationRepository.GetConversationsIdsByUserId(id).ToList();
            if (conversationsIds != null)
            {
                friendsIds = _conversationRepository.GetFriendsIdsList(conversationsIds, id);
            }
            return friendsIds;
        }

        //return Conversation by creator's id
        public Conversation GetConvByUserId(int userId)
        {
            var parts = _participantsRepository.GetParticipantsByUserId(userId);

            var conversation = _conversationRepository.GetByCreatorId(userId);
            return conversation;
        }


        //get conversationsdata for user
        public Tuple<List<EF.UserDetails>, List<int>> GetConversationByStringId(string stringId)
        {
            int id = _userService.ConvertId(stringId);
            var listFriendConvers = GetFriendsIdListFromConversation(id);
            var conversationdata = _userService.GetUserDetailsByListId(listFriendConvers);

            var conversation = _conversationRepository.GetConversationsIdsByUserId(id).ToList();

            return Tuple.Create<List<EF.UserDetails>, List<int>>(conversationdata, conversation);
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


        public void CreateParticipants(string currentUsId, int receiverId, int conversationId)
        {
            var curUserId = _userRepository.ReturnIntId(currentUsId);
            Participants participantCurUser = new Participants()
            {
                Conversation_id = conversationId,
                Users_id = curUserId
            };
            CreateParticipants(participantCurUser);

            Participants participantReceiver = new Participants()
            {
                Conversation_id = conversationId,
                Users_id= receiverId
            };
            CreateParticipants(participantReceiver);

        }



    }
}