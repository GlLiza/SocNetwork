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
        }


        //get all friends
        public IQueryable<Friendship> GetFriendForSelect(string id)
        {
            return friendshipRepository.GetListFriends(id);
        }


        //get UserDetails information for friends
        public List<FriendMsg> GetUserDetails(IQueryable<Friendship> friendships)
        {
            List<FriendMsg> friendDetails= new List<FriendMsg>();

            foreach (var friend in friendships)
            {
                FriendMsg user =new FriendMsg();
                var userDetails=userRepository.GetUserById(GetIntId(friend.Friend_id));
                var image = imagesRepository.GetImageById(userDetails.ImagesId);
                //if (userDetails != null && image != null)
                //{
                    user.Name = userDetails.Name;
                    user.FirstName = userDetails.Firstname;
                    user.Image = image.Data;
                    friendDetails.Add(user);
                //}
               
            }
            return friendDetails.ToList();
        }
        

        //translate Id-string -> Id-int
        public int GetIntId(string id)
        {
            return userRepository.ReturnIntId(id);
        }

    }
}