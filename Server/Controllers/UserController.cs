using Microsoft.AspNetCore.Mvc;
using SearchSniffServer.Models;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IRandomService randomService;

    public UserController(IUserService userService, IRandomService randomService) : base(userService)
    {
        this.randomService = randomService;
    }
    
    // #if DEBUG
    // [HttpPost(Name = "UserAdd")]
    // [Route("Add")]
    // public async Task<IActionResult> AddUser(AddUserRequest request)
    // {
    //     if (!await IsAdminAuthorized())
    //         return BadRequest("Not authorized");
    //     
    //     if (request.Name.Length < 3)
    //         return BadRequest("Name is too short");
    //     
    //     var password = randomService.GenerateRandomString(120);
    //     var result = await userService.AddUser(request.Name, password);
    //
    //     if (!result)
    //         return Problem();
    //     
    //     return Ok(password);
    // }
    // #endif
}