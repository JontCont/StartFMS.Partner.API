using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper
{
    public class LineBot:LineBots
    {
        public override void MessageText()
        {
            var @event = LineReceived.events.FirstOrDefault();
            string message = @event!=null ? @event.message.text:"";
            ReplyMessage(message);
        }
    }
}
