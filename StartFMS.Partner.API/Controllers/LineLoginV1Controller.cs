using Microsoft.AspNetCore.Mvc;
using StartFMS.Extensions.Line;
using StartFMS.Models.Backend;

namespace StartFMS.Partner.API.Controllers;

[ApiController]
[Route("/api/Line/Login/v1.0/")]
public class LineLoginV1Controller : ControllerBase
{
    private readonly ILogger<LineLoginV1Controller> _logger;
    private LineLogin _LineLogin;
    private readonly A00_BackendContext _backendContext;


    public LineLoginV1Controller(
        ILogger<LineLoginV1Controller> logger,
        LineLogin LineLogin,
        A00_BackendContext backendContext)
    {
        _logger = logger;
        _LineLogin = LineLogin;
        _backendContext = backendContext;
    }

    [HttpGet(Name = "")]
    public IActionResult ReceivedAuthorize([FromQuery] string code)
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
