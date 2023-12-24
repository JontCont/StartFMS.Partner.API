using isRock.LineBot;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper;

public partial class LineBot : LineBots
{
    public LineBot()
    {
    }

    public override async void MessageText()
    {
        //事件
        var @event = LineReceived?.events.FirstOrDefault();

        //取得留言字串
        string message = @event != null ? @event.message.text : "";

        // 回應訊息
        if (message.IndexOf("/test ") == 0)
        {
            string quest = message.Substring(6);
            ReplyMessage("目前還在測試階段，敬請期待");
        }
    }

    public override void MessageSticker()
    {
        var @event = LineReceived.events.FirstOrDefault();
        isRock.LineBot.Utility.ReplyStickerMessage(this.ReplyUserID, @event.message.packageId, @event.message.stickerId, this.ChannelToken);
    }


}
