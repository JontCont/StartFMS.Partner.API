using StartFMS.Extensions.Line;
using StartFMS.Partner.Line.WebAPI.Extensions.ChatGPT;

namespace StartFMS.Partner.API.Helper;

public partial class LineBot:LineBots
{
    public override async void MessageText()
    {
        //事件
        var @event = LineReceived.events.FirstOrDefault();
        
        //取得留言字串
        string message = @event!=null ? @event.message.text:"";

        // 回應訊息
        if(message.IndexOf("/chat ") == 0)
        {
            string quest = message.Substring(6) ;
            ReplyMessage(await Chat.ResponseMessageAsync(quest));
        }
        else
        {
            ReplyMessage(message);
        }
    }

    public override void MessageSticker()
    {
        var @event = LineReceived.events.FirstOrDefault();
        isRock.LineBot.Utility.ReplyStickerMessage(this.ReplyUserID,@event.message.packageId,@event.message.stickerId,this.ChannelToken);
        //base.MessageSticker();  
    }


}
