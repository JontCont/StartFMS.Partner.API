using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Partner.Line.WebAPI.Extensions.LineBots;
using StartFMS.Models.Backend;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Bot/v1.0/")]
public class LineBotsV1Controller : ControllerBase {
    private readonly ILogger<LineBotsV1Controller> _logger;
    private LineBots _lineBots;
    private readonly A00_BackendContext _backendContext;


    public LineBotsV1Controller(
        ILogger<LineBotsV1Controller> logger,
        LineBots lineBots,
        A00_BackendContext backendContext) {
        _logger = logger;
        _lineBots = lineBots;
        _backendContext = backendContext;
    }

    [HttpPost("", Name = "Message Reply")]
    public async Task<string> Post() {
        try {
            LineBots line = await _lineBots.LoadAsync(Request.Body);
            var LineEvent = line.ReceivedMessage.events.FirstOrDefault();
            string userId = LineEvent.source.userId;
            var lineOption = _backendContext.B10LineMessageOptions.Where(x => x.UserId == userId);
            TextMessage textMessage = new("");
            B10LineMessageOption b10Line = new B10LineMessageOption();
            switch (LineEvent.message.text) {
                case "�ؿ�":
                    textMessage = new TextMessage($"�п�ܤU��ﶵ (�ШϥΤ�����)");
                    if(!lineOption.Any() || lineOption.Where(x => x.Type == "ChatGPT" && x.IsUse.ToLower() == "false").Any()) {
                        textMessage.quickReply.items.Add(new QuickReplyMessageAction("�}�� ChatGPT ���", "�}�� ChatGPT ���"));
                    }
                    else {
                        textMessage.quickReply.items.Add(new QuickReplyMessageAction("���� ChatGPT ���", "���� ChatGPT ���"));
                    }

                    if (!lineOption.Any() || lineOption.Where(x => x.Type == "Reply" && x.IsUse.ToLower() == "false").Any()) {
                        textMessage.quickReply.items.Add(new QuickReplyMessageAction("�}�� �ҥ黡��", "�}�� �ҥ黡��"));
                    }
                    else {
                        textMessage.quickReply.items.Add(new QuickReplyMessageAction("���� �ҥ黡��", "���� �ҥ黡��"));
                    }
                    line.ReplyUserID = LineEvent.replyToken;
                    line.PushMessage(textMessage);
                    break;
                case "�}�� ChatGPT ���":
                    if (!lineOption.Any()) {
                        _backendContext.B10LineMessageOptions.Add(new B10LineMessageOption { UserId = userId, IsUse = true.ToString(), Type = "ChatGPT" });
                    }
                    else {
                        b10Line = lineOption.Where(x => x.Type == "ChatGPT").FirstOrDefault();
                        b10Line.IsUse = true.ToString();
                        _backendContext.B10LineMessageOptions.Update(b10Line);
                    }
                    await _backendContext.SaveChangesAsync();
                    break;
                case "�}�� �ҥ黡��":
                    if (!lineOption.Any()) {
                        _backendContext.B10LineMessageOptions.Add(new B10LineMessageOption { UserId = userId, IsUse = true.ToString(), Type = "Reply" });
                    }
                    else {
                        b10Line = lineOption.Where(x => x.Type == "Reply").FirstOrDefault();
                        b10Line.IsUse = true.ToString();
                        _backendContext.B10LineMessageOptions.Update(b10Line);
                    }
                    await _backendContext.SaveChangesAsync();
                    break;
                case "���� ChatGPT ���":
                    b10Line = lineOption.Where(x => x.Type == "ChatGPT").FirstOrDefault();
                    b10Line.IsUse = false.ToString();
                    _backendContext.B10LineMessageOptions.Update(b10Line);
                    await _backendContext.SaveChangesAsync();
                    break;
                case "���� �ҥ黡��":
                    b10Line = lineOption.Where(x => x.Type == "Reply").FirstOrDefault();
                    b10Line.IsUse = false.ToString();
                    _backendContext.B10LineMessageOptions.Update(b10Line);
                    await _backendContext.SaveChangesAsync();
                    break;
                default:
                    line.ReplyUserID = LineEvent.replyToken;
                    if (lineOption.Where(x => x.Type == "Reply" && x.IsUse.ToLower() == "true").Any()) {
                        textMessage = new TextMessage($"�z�����O : {LineEvent.message.text}");
                        line.ReplyMessage(textMessage.text);
                    }
                    if (lineOption.Where(x => x.Type == "ChatGPT" && x.IsUse.ToLower() == "true").Any()) {
                        string Prompt = LineEvent.message.text.ToString().Replace("!chat ", "");
                        string Request = await Line.WebAPI.Extensions.ChatGPT.Chat.ResponseMessageAsync(Prompt);
                        textMessage = new TextMessage(Request);
                        line.ReplyMessage(textMessage.text);
                    }
                    break;
            }

            return JsonConvert.SerializeObject(new {
                Success = true,
                Message = "",
            });
        }
        catch (Exception ex) {
            return JsonConvert.SerializeObject(new {
                Success = false,
                Message = ex.Message,
            });
        }
    }


}
