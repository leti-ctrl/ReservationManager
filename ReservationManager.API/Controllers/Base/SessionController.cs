using Microsoft.AspNetCore.Mvc;
using ReservationManager.Core.Dtos;

namespace ReservationManager.API.Controllers.Base;

public class SessionController : ControllerBase
{
    protected SessionInfo GetSession()
    {
        var realUser = HttpContext.Request.Query["email"];
        if (realUser.Count == 0) throw new Exception();
        return new SessionInfo(realUser.ToString());
    }
}