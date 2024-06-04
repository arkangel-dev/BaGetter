using BaGetter.Core;
using BaGetter.Core.Authentication;
using BaGetter.Web.DtoModels.AuthenticationModels;
using BaGetter.Web.DtoModels.StandardModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaGetter.Web.Controllers;

[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IContext _dbcontext;
    private readonly IUserManagementService _userManagementService;
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(
        IUserManagementService userManagementService,
        IAuthenticationService authenticationService,
        IContext dbcontext)
    {
        _userManagementService = userManagementService;
        _authenticationService = authenticationService;
        _dbcontext = dbcontext;
    }

    [HttpGet("VerifyToken")]
    public IActionResult VerifyToken(
        [FromBody]
        VerifyTokenReqModel payload)
    {
        var user = _dbcontext.Users.SingleOrDefault(x => x.Username == payload.Username);
        if (user is null)
            return NotFound(new StatusMessageModel("Username or token invalid"));

        if (user.Token != payload.Token)
            return NotFound(new StatusMessageModel("Username or token invalid"));

        return Ok(new StatusMessageModel("Token and username is valid"));
    }

    [HttpGet("AddNewUser")]
    public async Task<IActionResult> AddNewUser(
        [FromBody]
        AddNewUserReqModel payload,

        CancellationToken cancellationToken)
    {
        var existingUser = _dbcontext.Users.SingleOrDefault(x => x.Username == payload.Username);
        if (existingUser is not null)
            return BadRequest(new StatusMessageModel("A user with this name already exists"));

        if (string.IsNullOrWhiteSpace(payload.Username))
            return BadRequest(new StatusMessageModel("The username cannot be null or whitespace"));


        var newUser = new User()
        {
            Token = Helper.SecurityHelper.GenerateToken(),
            Username = payload.Username,
        };
        _dbcontext.Users.Add(newUser);
        await _dbcontext.SaveChangesAsync(cancellationToken);

        return Ok(new AddNewUserRespModel()
        {
            Token = newUser.Token,
            Username = newUser.Username,
        });
    }
}

