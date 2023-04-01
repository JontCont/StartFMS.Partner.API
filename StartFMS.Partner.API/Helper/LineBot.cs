using isRock.LineBot;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper
{
    public class LineBot:LineBots
    {
        public override void MessageText()
        {
            var @event = ReceivedMessage.events.FirstOrDefault();
            string message = @event!=null ? @event.message.text:"";
            ReplyMessage(message);
            //base.MessageText();
        }
    }
}
