using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Extensions.Line;
using StartFMS.Models.Backend;
using StartFMS.Partner.API.Helper;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Bot/v1.0/")]
public class LineBotsV1Controller : ControllerBase
{
    private readonly ILogger<LineBotsV1Controller> _logger;
    private LineBot _lineBots;
    private readonly A00_BackendContext _backendContext;


    public LineBotsV1Controller(
        ILogger<LineBotsV1Controller> logger,
        LineBot lineBots,
        A00_BackendContext backendContext) {
        _logger = logger;
        _lineBots = lineBots;
        _backendContext = backendContext;
    }

    [HttpPost("", Name = "Message Reply")]
    public async Task<string> Post() {

        try
        {
            using (var linebot = await _lineBots.LoadAsync(Request.Body))
            {
                linebot.ExecuteReader();
            }

            return JsonConvert.SerializeObject(new
            {
                Success = true,
                Message = "",
            });
        }
        catch(Exception ex) {
            return JsonConvert.SerializeObject(new
            {
                Success = false,
                Message = ex.Message,
            });
        }

    }



}
