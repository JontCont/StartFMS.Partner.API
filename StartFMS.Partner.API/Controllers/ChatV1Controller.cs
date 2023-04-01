using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Partner.Line.WebAPI.Extensions.ChatGPT;

namespace StartFMS.Partner.Line.WebAPI.Controllers;

[ApiController]
[Route("/api/Chat/v1.0/")]
public class ChatV1Controller : ControllerBase
{
    private readonly ILogger<ChatV1Controller> _logger;

    public ChatV1Controller(ILogger<ChatV1Controller> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<string> Post(string message)
    {
        await Chat.ResponseMessageAsync(message);
        return JsonConvert.SerializeObject(new {
            Success = true,
            Message = "",
        });
    }


}
