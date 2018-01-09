using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;


namespace network.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MessagesService _msgService;
        private readonly UserService _userService;
        private readonly ImageService _imgService;


        public MessagesController(MessagesService msgService, UserService userService, ImageService imgService)
        {
            _msgService = msgService;
            _userService = userService;
            _imgService = imgService;
        }

        // GET: Messages
        [HttpGet]
        public ActionResult Index()
        {
            List<IndexConversationViewModel> model = new List<IndexConversationViewModel>();
            var conversationdata = _msgService.GetConvData(User.Identity.GetUserId());
            model = conversationdata;
            return View(model);
        }


        //GET: select a receiver
        [HttpGet]
        public ActionResult SelectReceiver()
        {
            SelectReceiver receiver = new SelectReceiver();
            List<UserDetails> receiverList = _msgService.GetReceiverForSelect(User.Identity.GetUserId());

            if (receiverList != null)
                receiver.FriendsList = this._msgService.GetUserDetails(receiverList);

            return PartialView("_SelectReceiver", receiver);
        }

        //POST: create a chat
        [HttpPost]
        public HttpStatusCode SelectReceiver(ReceiverDto dto)
        {
            var conversation = _msgService.CreateConversation(User.Identity.GetUserId());
            _msgService.CreateParticipants(User.Identity.GetUserId(), dto.receiverId, conversation.Id);

            return HttpStatusCode.OK;
        }

        [HttpGet]
        public ActionResult OpenConversation(int? Conversation_id)
        {
            OpenConversationViewModel model = new OpenConversationViewModel();

            List<ConversationViewModel> listMembers = _msgService.GetMembersForParticipants(Conversation_id);
            model.Members = listMembers;

            List<MessageBlocks> blocksMsg = _msgService.GetMessagesForConversation(Conversation_id);
            model.Messages = blocksMsg;
            model.Conversation_id = Conversation_id;
            model.CurrentUserId = _userService.GetIntUserId(User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        public ActionResult SendMessage(MessageViewModel msg)
        {
            if (msg != null)
            {
                Messages message = new Messages()
                {
                    Conversation_id = msg.Conversation_id,
                    Sender_id = _userService.GetIntUserId(User.Identity.GetUserId()),
                    Message = msg.Message,
                    Created_at = DateTime.Now,
                    Visibility = true,
                    IsNotReading=true
                };

                _msgService.SendMsg(message);

                return RedirectToAction("OpenConversation", "Messages", new { Conversation_id = message.Conversation_id });
            }
            return RedirectToAction("Index", "Messages");
        }


        [HttpPost]
        public HttpStatusCode ReadingMsg(int? conId)
        {
            List<int> result = new List<int>();
            var listFull = _msgService.GetListIdOfMsg(conId);
            var curUserId = _userService.ConvertId(User.Identity.GetUserId());
            var listSorted =_msgService.SorteListMsg(curUserId, listFull);
            _msgService.ReadingMsg(listSorted);
            return HttpStatusCode.OK;
        }

        public ActionResult DeleteMessage(int msgId,int convId)
         {
            _msgService.DeleteMessage(msgId);
            return RedirectToAction("OpenConversation","Messages",new { Conversation_id= convId});
        }

        public string GetPhotoForAvatar(int senderId)
        {
            var sender = _userService.SearchUser(senderId);
            var senderImage = _imgService.GetProfilesPhoto(senderId);
            return Convert.ToBase64String(senderImage);
        }




        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    //Log the error!!
        //    Console.WriteLine(filterContext.Exception.Message);
        //    //_Logger.Error(filterContext.Exception);

        //    //Redirect or return a view, but not both.
        //    filterContext.Result = RedirectToAction("Index", "ErrorHandler");
        //    // OR 
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/ErrorHandler/Index.cshtml"
        //    };
        //}


        //protected List<MessageBlocks> CheckMessages(List<MessageBlocks> listMsg)
        //{
        //    string stringId = User.Identity.GetUserId();
        //    var intId = _userService.GetIntUserId(stringId);

        //    foreach (var item in listMsg)
        //    {
        //        if (item.SenderId != intId && item.VisibForInterlocutor == false)
        //        {

        //        }
        //    }
        //}
    }

    public class ReceiverDto
    {
        public int receiverId { get; set; }
    }

   
    
}
     