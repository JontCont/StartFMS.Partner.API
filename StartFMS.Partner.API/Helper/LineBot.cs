using isRock.LineBot;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Helper;

public partial class LineBot : LineBots
{
    private readonly OpenAIService _openAIService;
    public LineBot(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public override async void MessageText()
    {
        //事件
        var @event = LineReceived?.events.FirstOrDefault();

        //取得留言字串
        string message = @event != null ? @event.message.text : "";

        // 回應訊息
        if (message.IndexOf("/chat ") == 0)
        {
            string quest = message.Substring(6);
            ReplyMessage(await ChatGpt_MessageAsync(quest));
        }
        if (message.IndexOf("/image ") == 0)
        {
            string quest = message.Substring(7);
            ReplyImage(await ChatGpt_ImageAsync(quest));
        }

        if (message.IndexOf("/test ") == 0)
        {
            string quest = message.Substring(6);
            ReplyMessage("目前還在測試階段，敬請期待");
        }
    }

    private async Task<string> ChatGpt_MessageAsync(string message)
    {
        var completionResult =
            await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = message,
                Model = OpenAI.GPT3.ObjectModels.Models.TextDavinciV3,
                MaxTokens = 4000
            });
        return await ChatAIModule.ChatMessageAsync(completionResult); ;
    }

    private async Task<string> ChatGpt_ImageAsync(string message)
    {
        var imageResult =
            await _openAIService.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = message,
                N = 1,
                Size = StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            });

        return ChatAIModule.ChatImage(imageResult).FirstOrDefault() ?? "";
    }
    public override void MessageSticker()
    {
        var @event = LineReceived.events.FirstOrDefault();
        isRock.LineBot.Utility.ReplyStickerMessage(this.ReplyUserID, @event.message.packageId, @event.message.stickerId, this.ChannelToken);
        //base.MessageSticker();  
    }


}
