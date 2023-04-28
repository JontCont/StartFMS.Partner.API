using isRock.LineBot;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper;

public partial class LineBot:LineBots
{
    private readonly OpenAIService _openAIService;
    public LineBot(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public override async void MessageText()
    {
        //事件
        var @event = LineReceived.events.FirstOrDefault();
        
        //取得留言字串
        string message = @event!=null ? @event.message.text:"";

        // 回應訊息
        ReplyMessage(await ChatGpt_MessageAsync(message));
    }

    public async Task<string> ChatGpt_MessageAsync(string message)
    {
        var completionResult = _openAIService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
        {
            Prompt = message,
            MaxTokens = 4000
        }, OpenAI.GPT3.ObjectModels.Models.TextDavinciV3);

        string str = "";
        await foreach (var completion in completionResult)
        {
            if (completion.Successful)
            {
                Console.Write(completion.Choices.FirstOrDefault()?.Text);
                str += completion.Choices.FirstOrDefault()?.Text;
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
            }
        }

        return str;
    }

    public override void MessageSticker()
    {
        var @event = LineReceived.events.FirstOrDefault();
        isRock.LineBot.Utility.ReplyStickerMessage(this.ReplyUserID,@event.message.packageId,@event.message.stickerId,this.ChannelToken);
        //base.MessageSticker();  
    }


}
