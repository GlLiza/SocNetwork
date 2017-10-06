using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using network.Views.ViewModels;

namespace network.BLL
{
    public class MessagesService
    {
        private NetworkContext db = new NetworkContext();
        private IMessageRepository messageRepository;
        private IParticipantsRepository participantsRepository;
        private IConversationRepository conversationRepository;
        private IFriendshipRepository friendshipRepository;
        private IUserRepository userRepository;
        private IImagesRepository imagesRepository;

        public RepositoryBase reposBase;


        public MessagesService()
        {
            participantsRepository=new ParticipantsRepository(db);
            conversationRepository=new ConversationRepository(db);
            messageRepository =new MessagesRepository(db);
            friendshipRepository = new FriendshipRepository(db);
            userRepository=new UserRepository(db);
            imagesRepository=new ImagesRepository(db);
            reposBase=new RepositoryBase(db);
        }


        //get all friends
        public IQueryable<Friendship> GetFriendForSelect(string id)
        {
            return friendshipRepository.GetListFriends(id);
        }

        //get list of friend's Ids without existing conversations
        public List<int> GetFriendsIdsList(int id)
        {
            List<int> friendsIds=new List<int>();

            var conversationIdsList = conversationRepository.GetConversationsIdsByCreatorId(id);
            if(conversationIdsList!=null)
                friendsIds=conversationRepository.GetFriendsIdsList(conversationIdsList);
            return friendsIds;
        }







        //get  information for friends
        public List<ConversationViewModel> GetUserDetails(List<UserDetails> userList)
        {
            List<ConversationViewModel> detailsList= new List<ConversationViewModel>();

            foreach (var item in userList)
            {
                ConversationViewModel user =new ConversationViewModel();
                var userDetails=userRepository.GetUserById(item.Id);
                var image = imagesRepository.GetImageById(userDetails.ImagesId);
                
                    user.Id = userDetails.Id;
                    user.Name = userDetails.Name;
                    user.FirstName = userDetails.Firstname;
                    user.Image = image.Data;
                    detailsList.Add(user);
            }
            return detailsList.ToList();
        }
        

        //translate Id-string -> Id-int
        public int GetIntId(string id)
        {
            return userRepository.ReturnIntId(id);
        }



        




        //  CONVERSATION
        public void CreateConversation(Conversation conversation)
        {
            if (conversation != null)
            {
                conversationRepository.AddConversations(conversation);
                reposBase.Save();

            }
        }


        //позволяет получить список id друзей, с которыми существует Conversation
        public List<int> GetFriendsIdListFromConversation(int id)
        {
            List<int> friendsIds=new List<int>();

            var conversationsIds = conversationRepository.GetConversationsIdsByCreatorId(id);
            if (conversationsIds != null)
            {
               friendsIds = conversationRepository.GetFriendsIdsList(conversationsIds);
            }
            return friendsIds;
        }


        //PARTICIPANTS

        public void CreateParticipants(Participants participants)
       {
            if (participants != null)
            {
                participantsRepository.AddParticipants(participants);
                reposBase.Save();
            }
            
        }

    }
}