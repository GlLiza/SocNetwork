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
            _imgRepository = imgRepository;
            _msgRepository = msgRepository;
            _userService = userService;
        }


        //get all friends
        public IQueryable<Friendship> GetFriendForSelect(string id)
        {
            return _friendshipRepository.GetListFriends(id);
        }
        //get  information for friends


        public List<ConversationViewModel> GetUserDetails(List<UserDetails> userList)
        {
            List<ConversationViewModel> detailsList = new List<ConversationViewModel>();

            foreach (var item in userList)
            {
                ConversationViewModel user = new ConversationViewModel();
                var userDetails = _userRepository.GetUserById(item.Id);
                var image = _imgRepository.GetImageById(userDetails.ImagesId);

                user.Id = userDetails.Id;
                user.FirstName = userDetails.Name;
                user.LastName = userDetails.Firstname;
                user.Image = Convert.ToBase64String(image.Data);
                detailsList.Add(user);
            }
            return detailsList.ToList();
        }


        // data for select receiver 
        public List<UserDetails> GetReceiverForSelect(string id)
        {
            var listIdStr = _friendshipRepository.GetListFriendsId(id);
            var intIds = _userService.ConvertListIds(id, listIdStr);
            var listFrConversation = GetFriendsIdListFromConversation(intIds.Item1);
            var dataOfReceiver = _userService.GetDataForSearch(intIds.Item2, listFrConversation);

            return dataOfReceiver;
        }



        //  CONVERSATION

        //create conversation + create participant
        public Conversation CreateConversation(string id)
        {
            Conversation conversation = new Conversation()
            {
                Creator_id = _userRepository.ReturnIntId(id),
                Created_at = DateTime.Now.Date,
                Visibility = true
            };
            _conversationRepository.AddConversations(conversation);

            return conversation;
        }


        //позволяет получить список id друзей, с которыми существует Conversation
        public List<int> GetFriendsIdListFromConversation(int id)
        {
            List<int> friendIds = new List<int>();
            var conversationsIds = _conversationRepository.GetConversationsIdsByUserId(id).ToList();

            if (conversationsIds != null)
            {
                friendIds = _conversationRepository.GetFriendsIdsList(conversationsIds, id);
            }
            return friendIds;
        }

        //return Conversation by creator's id
        public Conversation GetConvByUserId(int userId)
        {
            var parts = _participantsRepository.GetParticipantsByUserId(userId);

            var conversation = _conversationRepository.GetByCreatorId(userId);
            return conversation;
        }


        //get conversationsdata for user
        public List<IndexConversationViewModel> GetConvData(string stringId)
        {
            int id = _userService.ConvertId(stringId);
            var listFriendConvers = GetFriendsIdListFromConversation(id);
            var friendsData = _userService.GetUserDetailsByListId(listFriendConvers);

            var converIds = _conversationRepository.GetConversationsIdsByUserId(id).ToList();

            List<IndexConversationViewModel> model = new List<IndexConversationViewModel>();

            foreach (var conId in converIds)
            {
                IndexConversationViewModel item = new IndexConversationViewModel();
                item.Conversation_id = conId;

                var con = _conversationRepository.GetConversationById(conId);

                foreach (var convData in friendsData)
                {
                    if (con.Visibility !=false)
                    {
                        var part = _participantsRepository.GetParticipantsByConversId(con.Id);
                        foreach (var p in part)
                        {
                            if (p.Users_id == convData.Id)
                            {
                                ConversationViewModel data = new ConversationViewModel()
                                {
                                    Id = convData.Id,
                                    FirstName = convData.Firstname,
                                    LastName = convData.Name,
                                    Image = Convert.ToBase64String(convData.Images.Data)
                                };
                                item.Conversation = data;
                                model.Add(item);
                            }
                        }
                    }
                    
                }
            }
            return model;
        }

        public bool CheckUnansweredMsgInConvers(int? conversId, int userId)
        {
            List<int> listNotReading = new List<int>();
            var listAllMsg = _msgRepository.GetListMessagesByConversationId(conversId);
            foreach (var item in listAllMsg)
            {
                if (item.IsNotReading == true && item.Sender_id != userId)
                    listNotReading.Add(item.Id);
            }

            if (listNotReading.Count > 0)
                return true;
            return false;
        }

        public List<int> GetIdsUnansweredConvers(IQueryable<int> converslis, int id)
        {
            List<int> result = new List<int>();

            foreach (var item in converslis)
            {
                var check = CheckUnansweredMsgInConvers(item, id);
                if (check)
                    result.Add(item);
            }
            return result;

        }


        public List<int> CountUnansweredConvers(string stringId)
        {
            List<int> listConvId = new List<int>();
            var intId = _userRepository.ReturnIntId(stringId);
            IQueryable<int> converslis = _conversationRepository.GetConversationsIdsByUserId(intId);
            if (converslis != null)
            {
                listConvId = GetIdsUnansweredConvers(converslis, intId);
            }
            return listConvId;
        }

        public Conversation GetConversById(int conversId)
        {
            var conv = _conversationRepository.GetConversationById(conversId);
            return conv;
        }

        public void DeleteConvers(Conversation conv)
        {
            var convers = _conversationRepository.GetConversationById(conv.Id);
            convers.Visibility=false;
            convers.Update_at = DateTime.Now;
            _conversationRepository.UpdateConversations(convers);
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
                item.MsgId = msg.Id;
                item.Message = msg.Message;
                item.Time = msg.Created_at;
                item.SenderId = msg.Sender_id;
                item.IsNotReading = msg.IsNotReading;

                var user = _userRepository.GetUserById(msg.Sender_id);
                var img = _imgRepository.GetImageById(user.Id);
                
                blockMsg.Add(item);
            }

            return blockMsg;
        }



        //MESSAGES
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


        public void ReadingMsg(List<int> ids)
        {
            var listMsg = GetListMsgByListId(ids);
            if (listMsg.Count>0)
                Reading(listMsg);
        }

        public List<Messages> GetListMsgByListId(List<int> ids)
        {
            List<Messages> list = new List<Messages>();
            foreach (var item in ids)
            {
                var msg = _msgRepository.FindMsg(item);
                list.Add(msg);
            }
            return list;
        }

        public void Reading(List<Messages> list)
        {
            foreach (var item in list)
            {
                item.IsNotReading = false;
                _msgRepository.UpdateMessage(item);
            }
        }


        public List<int> GetListIdOfMsg (int? conversId)
        {
            var msgs = _msgRepository.GetListMessagesByConversationId(conversId);
            var list = _msgRepository.GetNotReadingMsg(msgs);
            return list;
        }

        public void DeleteMessage(int id)
        {
            var msg = _msgRepository.FindMsg(id);
            msg.Visibility =false;
            _msgRepository.UpdateMessage(msg);
        }

        public List<int> SorteListMsg(int userId,List<int>list)
        {
            List<int> result = new List<int>();
            foreach (var item in list)
            {
                var msg = _msgRepository.FindMsg(item);
                if (msg.Sender_id != userId)
                    result.Add(msg.Id);
            }

            return result;
        }

        public List<int> CountUnansweredMessages(int conversId, int userId)
        {
            List<int> result = new List<int>(); 
            var listMsg = _msgRepository.GetListMessagesByConversationId(conversId);
            foreach (var item in listMsg)
            {
                if (item.IsNotReading == true && item.Sender_id!= userId)
                    result.Add(item.Id);
            }
            return result; 
        }

    }
}