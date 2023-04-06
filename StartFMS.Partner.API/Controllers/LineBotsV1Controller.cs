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
    private LineBot _lineBots;

    public LineBotsV1Controller(LineBot lineBots) {
        _lineBots = lineBots;
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
