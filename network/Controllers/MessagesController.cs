using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using network.Views.ViewModels;


namespace network.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessagesService _msgService;
        private readonly UserService _userService;
        private readonly FriendshipService _friendService;
        private readonly ImageService _imgService;

        public MessagesController()
        {
        }

        public MessagesController(MessagesService msgService, UserService userService, FriendshipService friendService, ImageService imgService)
        {
            _msgService = msgService;
            _userService = userService;
            _friendService = friendService;
            _imgService = imgService;
        }

        // GET: Messages
        public ActionResult Index()
        {
            List<ConversationViewModel> model=new List<ConversationViewModel>();
            var a = ConversationList(model);
            return View(a);
        }


        //GET: select a receiver
        [HttpGet]
        public ActionResult SelectReceiver()
        {
            SelectReceiver receiver = new SelectReceiver();
            List<UserDetails> receiverList = GetFriendsForSearch();

            if (receiverList != null)
                receiver.FriendsList = this._msgService.GetUserDetails(receiverList);

            return PartialView("_SelectReceiver", receiver);
        }


        //POST: create a chat
        [HttpPost]
        public String SelectReceiver(int receiverId)
        {
            var conversation = CreateConversation();
            if (receiverId != 0)
                CreateParticipants(receiverId, conversation.Id);


            JavaScriptSerializer js = new JavaScriptSerializer();
            var res = new HttpStatusCodeResult(HttpStatusCode.OK);
            return js.Serialize(res);
        }

        



        public List<UserDetails> GetFriendsForSearch()
        {
            string strId = User.Identity.GetUserId();
            var listIdsString = this._friendService.GetFriendsIdsList(strId);

            var intIds = this._userService.ConvertListIds(strId, listIdsString);   

            var listFriendConvers =this._msgService.GetFriendsIdListFromConversation(intIds.Item1);

            var dataForReceiver = this._userService.GetDataForSearch(intIds.Item2, listFriendConvers);

            return dataForReceiver;
        }
        
        public Conversation CreateConversation()
        {
            Conversation conversation = new Conversation();

            conversation.Creator_id = this._msgService.GetIntId(User.Identity.GetUserId());
            conversation.Created_at = DateTime.Now.Date;
            this._msgService.CreateConversation(conversation);

            return conversation;
        }

        public Participants CreateParticipants(int receiverId, int conversationId)
        {
            Participants participants = new Participants();

            if (receiverId>0 && conversationId> 0)
            {
                participants.Conversation_id = conversationId;
                participants.Users_id = receiverId;

                this._msgService.CreateParticipants(participants);
            }
            return participants;
        }

        public List<ConversationViewModel> ConversationList(List<ConversationViewModel> model)
        {
            int id = _userService.CovertId(User.Identity.GetUserId());

            var listFriendConvers = this._msgService.GetFriendsIdListFromConversation(id);

            var conversationdata = this._userService.GetUserDetailsByListId(listFriendConvers);


            foreach (var user in conversationdata)
            {
                var friend=new ConversationViewModel();

                var image = this._imgService.SearchImg(user.ImagesId);

                friend.Id = user.Id;
                friend.Name = user.Name;
                friend.FirstName = user.Firstname;
                friend.Image = image.Data;

                model.Add(friend);
            }

            return model;
        }
        
    }
}
