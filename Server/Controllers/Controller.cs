using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

public abstract class Controller : ControllerBase
{
    private readonly IUserService userService;

    public Controller(IUserService userService)
    {
        this.userService = userService;
    }
    
    protected async Task<bool> IsAuthorized()
    {
        if (!Request.Headers.TryGetValue("x-user", out var user))
            return false;
        
        if (!Request.Headers.TryGetValue("x-user-token", out var token))
            return false;

        return await userService.VerifyUser(user, token);
    }
    
    protected async Task<bool> IsAdminAuthorized()
    {
        if (!Request.Headers.TryGetValue("x-user", out var user))
            return false;
        
        if (!Request.Headers.TryGetValue("x-user-token", out var token))
            return false;

        return await userService.VerifyUser(user, token) && await userService.IsAdmin(user);
    }
}