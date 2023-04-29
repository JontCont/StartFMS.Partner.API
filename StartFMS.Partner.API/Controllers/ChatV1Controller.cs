using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using StartFMS.Partner.API.Helper;

namespace StartFMS.Partner.Line.WebAPI.Controllers;

[ApiController]
[Route("/api/Chat/v1.0/")]
public class ChatV1Controller : ControllerBase
{
    private readonly ILogger<ChatV1Controller> _logger;
    private readonly OpenAIService _openAIService;

    public ChatV1Controller(ILogger<ChatV1Controller> logger, OpenAIService openAIService)
    {
        _logger = logger;
        _openAIService = openAIService;
    }

    [HttpGet]
    public async Task<string> ChatGptMessage(string message)
    {
        var completionResult =
            await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = message,
                Model = OpenAI.GPT3.ObjectModels.Models.TextDavinciV3,
                MaxTokens = 4000
            });

        return JsonConvert.SerializeObject(new
        {
            Success = true,
            Message = await ChatAIModule.ChatMessageAsync(completionResult),
        });
    }

    [HttpGet("Image")]
    public async Task<string> ChatGptImage(string message)
    {
        var imageResult = await _openAIService.Image.CreateImage(new ImageCreateRequest
        {
            Prompt = message,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size256,
            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
        });

        return JsonConvert.SerializeObject(new
        {
            Success = true,
            Message = ChatAIModule.ChatImage(imageResult),
        });
    }

}
