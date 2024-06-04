using BaGetter.Core;
using BaGetter.Web.DtoModels.AuthenticationModels;
using BaGetter.Web.DtoModels.PackageModels;
using BaGetter.Web.DtoModels.StandardModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaGetter.Web.Controllers;

[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IContext _dbcontext;
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(
        IAuthenticationService authenticationService,
        IContext dbcontext)
    {
        _authenticationService = authenticationService;
        _dbcontext = dbcontext;
    }

    [HttpPost("Users/VerifyToken")]
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

    [HttpPut("Users")]
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
            IsAdmin = payload.IsAdmin,
        };
        _dbcontext.Users.Add(newUser);
        await _dbcontext.SaveChangesAsync(cancellationToken);

        return Ok(new AddNewUserRespModel()
        {
            Token = newUser.Token,
            Username = newUser.Username,
        });
    }

    [HttpGet("Users")]
    public async Task<IActionResult> ListAllUsers(CancellationToken cancellationToken)
    {

        return Ok(_dbcontext.Users.Select(x => new UserModel()
        {
            IsAdmin = x.IsAdmin,
            Username = x.Username,
            Packages = x.Packages.Select(x => x.Title + ":" + x.OriginalVersionString).ToArray()
        }));
    }

    [HttpPatch("Packages/Ownership")]
    public async Task<IActionResult> ChangeOwnership(
        [FromBody]
        ReassignPackageRequestModel request,
        CancellationToken cancellationToken)
    {
        var newOwner = _dbcontext.Users.SingleOrDefault(x => x.Username == request.Assignee);
        if (newOwner is null && request.Assignee is not null)
            return NotFound(new StatusMessageModel($"The user '{request.Assignee}' was not found"));
        
        var packages = _dbcontext.Packages
            .Where(x => x.Id == request.PackageId)
            .Include(x => x.Owner)
            .AsQueryable();
        if (request.Versions is not null)
            packages = packages.Where(x => request.Versions.Contains(x.NormalizedVersionString));

       

        var finalPackages = packages.ToList();
        foreach (var package in finalPackages)
        {
            package.Owner = newOwner;
        }

        await _dbcontext.SaveChangesAsync(cancellationToken);

        return Ok(new ReassignPackageResponseModel()
        {
            Assignee = request.Assignee,
            PackageId = request.PackageId,
            ReassignedCount = packages.Count()
        });
    }
}

