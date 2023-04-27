using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

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

    [HttpPost]
    public async Task<string> Post(string message)
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
        _logger.LogInformation($" 最後回答結果 {str}");

        return JsonConvert.SerializeObject(new {
            Success = true,
            Message = "",
        });
    }


}
