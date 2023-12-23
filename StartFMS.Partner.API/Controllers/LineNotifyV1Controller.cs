using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Notify/v1.0/")]
public class LineNotifyV1Controller : ControllerBase
{
    private LineNotify _LineNotify;

    public LineNotifyV1Controller(LineNotify LineNotify)
    {
        _LineNotify = LineNotify;
    }

    [HttpGet("Developer")]
    public string DeveloperSendMessage()
    {
        _LineNotify.DeveloperSend($"發送訊息時間 : {DateTime.Now}");
        return JsonConvert.SerializeObject(new
        {
            Success = true,
            Message = ""
        });
    }


    [HttpGet]
    public IActionResult SendMessage([FromQuery] string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            string Url = _LineNotify.GetNotifyUrl();
            return Redirect(Url); //發送網址
        }
        try
        {
            var token = _LineNotify.GetTokenFromCode(code);
            _LineNotify.Send($"發送訊息時間 : {DateTime.Now}", token.access_token, 446, 1988);
        }
        catch
        {
            return Ok(new { sccess = false, message = "尚未加入 Notify 到頻道中" });
        }


        return Ok(new { sccess = true });
    }
}
