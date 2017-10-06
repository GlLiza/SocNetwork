using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;

namespace network.Controllers
{
    public class MessagesController : Controller
    {

        public MessagesService msgService;
        public UserService userService;
        public FriendshipService friendService;
        public ImageService imgService;



        public MessagesController()
        {
            msgService = new MessagesService();
            userService = new UserService();
            friendService=new FriendshipService();
            imgService = new ImageService();
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
                receiver.FriendsList = msgService.GetUserDetails(receiverList);

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


        //[HttpGet]
        //public ActionResult Messages(int id)
        //{
        //    var friend = userService.SearchUser(id);
        //    MessagesViewModel model=new MessagesViewModel();
        //    model.NameSender = friend.Name;
        //    model.FirstNameSender = friend.Firstname;
        //    model.Image = imgService.ReturnImage(friend.ImagesId.Value);

        //}

        //[HttpPost]
        //public ActionResult Messages()
        //{

        //}











        public List<UserDetails> GetFriendsForSearch()
        {
            string strId = User.Identity.GetUserId();
            var listIdsString = friendService.GetFriendsIdsList(strId);

            var intIds = userService.ConvertListIds(strId, listIdsString);   

            var listFriendConvers =
                msgService.GetFriendsIdListFromConversation(intIds.Item1);

            var dataForReceiver = userService.GetDataForSearch(intIds.Item2, listFriendConvers);

            return dataForReceiver;
        }
        
        public Conversation CreateConversation()
        {
            Conversation conversation = new Conversation();

            conversation.Creator_id = msgService.GetIntId(User.Identity.GetUserId());
            conversation.Created_at = DateTime.Now.Date;
            msgService.CreateConversation(conversation);

            return conversation;
        }

        public Participants CreateParticipants(int receiverId, int conversationId)
        {
            Participants participants = new Participants();

            if (receiverId>0 && conversationId> 0)
            {
                participants.Conversation_id = conversationId;
                participants.Users_id = receiverId;

                msgService.CreateParticipants(participants);
            }
            return participants;
        }

        public List<ConversationViewModel> ConversationList(List<ConversationViewModel> model)
        {
            int id = userService.CovertId(User.Identity.GetUserId());

            var listFriendConvers = msgService.GetFriendsIdListFromConversation(id);

            var conversationdata = userService.GetUserDetailsByListId(listFriendConvers);


            foreach (var user in conversationdata)
            {
                ConversationViewModel friend=new ConversationViewModel();

                var image = imgService.SearchImg(user.ImagesId);

                friend.Id = user.Id;
                friend.Name = user.Name;
                friend.FirstName = user.Firstname;
                friend.Image = image.Data;

                model.Add(friend);
            }

            return model;
        }












        //// GET: Messages/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Messages/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Messages/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Messages/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Messages/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Messages/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Messages/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
