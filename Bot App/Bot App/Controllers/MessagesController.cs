using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Web.Services.Description;
using System.Collections.Generic;

namespace Bot_App
{
    [BotAuthentication]
    //public class MessagesController : ApiController
    //{
    //    /// <summary>
    //    /// POST: api/Messages
    //    /// Receive a message from a user and reply to it
    //    /// </summary>
    //    public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
    //    {
    //        if (activity.Type == ActivityTypes.Message)
    //        {
    //            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
    //            // calculate something for us to return
    //            int length = (activity.Text ?? string.Empty).Length;

    //            // return our reply to the user
    //            Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
    //            await connector.Conversations.ReplyToActivityAsync(reply);
    //        }
    //        else
    //        {
    //            HandleSystemMessage(activity);
    //        }
    //        var response = Request.CreateResponse(HttpStatusCode.OK);
    //        return response;
    //    }

    //    private Activity HandleSystemMessage(Activity message)
    //    {
    //        if (message.Type == ActivityTypes.DeleteUserData)
    //        {
    //            // Implement user deletion here
    //            // If we handle user deletion, return a real message
    //        }
    //        else if (message.Type == ActivityTypes.ConversationUpdate)
    //        {
    //            // Handle conversation state changes, like members being added and removed
    //            // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
    //            // Not available in all channels
    //        }
    //        else if (message.Type == ActivityTypes.ContactRelationUpdate)
    //        {
    //            // Handle add/remove from contact lists
    //            // Activity.From + Activity.Action represent what happened
    //        }
    //        else if (message.Type == ActivityTypes.Typing)
    //        {
    //            // Handle knowing tha the user is typing
    //        }
    //        else if (message.Type == ActivityTypes.Ping)
    //        {
    //        }

    //        return null;
    //    }
    //}

    public class flavor
    {
        public flavor(string flavor, string message)
        {
            Flavor = flavor;
            Message = message;
        }

        public string Flavor { get; set; }
        public string Message { get; set; }
    }


    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            List<flavor> choices = new List<flavor>();
            choices.Add(new flavor("chocolate", "CH"));
            choices.Add(new flavor("vanilla", "VA"));
            choices.Add(new flavor("mint chip", "MC"));

            Message replyMessage = message.CreateReplyMessage();
            replyMessage.Language = "en";

            if (message.Type == "Message")
            {
                replyMessage.Text = "Choose a favorite ice cream flavor";

                //create the attachment list, and an attachment to hold the options.
                replyMessage.Attachments = new List<Attachment>();
                Attachment messageOptions = new Attachment();

                //create the list of Actions the user might take (representing the buttons), and populate the list
                messageOptions.Actions = new List<Microsoft.Bot.Connector.Action>();
                foreach (flavor choice in choices)
                {
                    messageOptions.Actions.Add(new Microsoft.Bot.Connector.Action(title: choice.Flavor, message: choice.Message));
                }

                //associate the Attachments List with the Message and send it
                replyMessage.Attachments.Add(messageOptions);

                return replyMessage;
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

    }

}