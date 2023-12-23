using Microsoft.AspNetCore.Mvc;
using StartFMS.Extensions.Line;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Login/v1.0/")]
public class LineLoginV1Controller : ControllerBase
{
    private LineLogin _LineLogin;

    public LineLoginV1Controller(LineLogin LineLogin)
    {
        _LineLogin = LineLogin;
    }

    [HttpGet(Name = "")]
    public IActionResult ReceivedAuthorize([FromQuery] string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            string Url = _LineLogin.GetLoginUrl();
            return Redirect(Url); //µo°eºô§}

        }
        var token = _LineLogin.GetTokenFromCode(code);
        var profile = _LineLogin.GetUserProfile(token.access_token);
        return Ok(new { sccess = true, token, profile });
    }

}
