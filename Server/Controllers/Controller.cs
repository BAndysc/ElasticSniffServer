using Microsoft.AspNetCore.Mvc;
using Server.Database.Models;
using Server.Services;

namespace Server.Controllers;

public abstract class Controller : ControllerBase
{
    protected readonly IUserService userService;

    public Controller(IUserService userService)
    {
        this.userService = userService;
    }

    protected bool GetHeaderUser(out string user)
    {
        if (Request.Headers.TryGetValue("x-user", out var u))
        {
            user = u;
            return true;
        }

        user = "";
        return false;
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
    
    protected async Task<UserModel?> GetUser()
    {
        if (!Request.Headers.TryGetValue("x-user", out var user))
            return null;
        
        if (!Request.Headers.TryGetValue("x-user-token", out var token))
            return null;

        if (!await userService.VerifyUser(user, token))
            return null;
        
        return await userService.GetUser(user);
    }
}