using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using TimeZoneBot.Services;

namespace TimeZoneBot.Controllers
{
   // [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type != "Message") return HandleSystemMessage(message);
            // return await Conversation.SendAsync(message, MakeRoot);
            var getMessage = await WeatherInfo(message.Text);
            var replyMessage = message.CreateReplyMessage(getMessage);
            return replyMessage;

        }

        private static async Task<string> WeatherInfo(string zoneName)
        {
            var response = await TimeZoneService.GetWeatherInfo(zoneName);
            string strMessage;

            if (response.Status == "OK")
            {
                var currentTime = response.Timestamp;
                var dateTime = ConvertFromUnixTimestamp(currentTime);

                strMessage = string.Format("The current Time  for {0} is {1}  ", zoneName, dateTime);
                return strMessage;
            }
            strMessage = string.Format("Please check the zone format {0}", zoneName);
            return strMessage;
        }

        private static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private Message HandleSystemMessage(Message message)
        {
            switch (message.Type)
            {
                case "Ping":
                    var reply = message.CreateReplyMessage();
                    reply.Type = "Ping";
                    return reply;
                case "DeleteUserData":
                    // Implement user deletion here
                    // If we handle user deletion, return a real message
                    break;
                case "BotAddedToConversation":
                    break;
                case "BotRemovedFromConversation":
                    break;
                case "UserAddedToConversation":
                    break;
                case "UserRemovedFromConversation":
                    return message.CreateReplyMessage("Error Occured.");
                case "EndOfConversation":
                    break;
            }

            return null;
        }
    }
}