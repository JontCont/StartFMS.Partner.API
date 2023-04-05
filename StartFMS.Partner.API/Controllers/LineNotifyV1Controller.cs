using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Extensions.Line;
using StartFMS.Models.Backend;
using StartFMS.Partner.API.Helper;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Notify/v1.0/")]
public class LineNotifyV1Controller : ControllerBase
{
    private readonly ILogger<LineNotifyV1Controller> _logger;
    private LineNotify _LineNotify;
    private readonly A00_BackendContext _backendContext;


    public LineNotifyV1Controller(
        ILogger<LineNotifyV1Controller> logger,
        LineNotify LineNotify,
        A00_BackendContext backendContext)
    {
        _logger = logger;
        _LineNotify = LineNotify;
        _backendContext = backendContext;
    }

    [HttpGet]
    public string SendMessage()
    {
        _LineNotify.Send($"發送訊息時間 : {DateTime.Now}");
        return JsonConvert.SerializeObject(new
        {
            Success = true,
            Message = ""
        });
    }
}
