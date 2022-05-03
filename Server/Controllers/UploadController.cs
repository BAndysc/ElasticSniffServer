using Microsoft.AspNetCore.Mvc;
using SearchSniffServer.Models;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : Controller
{
    private readonly IUploadService uploadService;

    public UploadController(IUserService userService, IUploadService uploadService) : base(userService)
    {
        this.uploadService = uploadService;
    }
    
    [HttpPost(Name = "Upload")]
    public async Task<IActionResult> Post(UploadSniffRequest request)
    {
        if (!await IsAdminAuthorized())
            return BadRequest("Not authorized");

        var user = await GetUser();

        if (user == null)
            return BadRequest("Bad user");
        
        var result = await uploadService.Upload(user, request);

        if (!result)
            return Problem("Couldn't upload the sniff");
        
        return Ok("Ok");
    }
}