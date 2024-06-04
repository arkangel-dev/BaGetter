using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Core.Authentication;
public interface IUserManagementService
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="userid">User ID of the user</param>
    /// <returns>True if the operation was successful</returns>
    public Task<bool> CreateUserAsync(string userid);

    /// <summary>
    /// Refresh the token of a user
    /// </summary>
    /// <param name="userid">Id of the user whose token needs to be refreshed</param>
    /// <returns>The newly generated token</returns>
    public Task<string> RefreshTokenAsync(string userid);

    /// <summary>
    /// Set the owner of a package
    /// </summary>
    /// <param name="id">ID of the package to re-assign ownership</param>
    /// <param name="userid">Owner of the package. If the value is null then the package is set as orphaned</param>
    /// <returns></returns>
    public Task<bool> SetPackageOwner(string id, string? userid);

    public Task<User?> GetUserAsync(string id);
}

public class UserManagementService : IUserManagementService
{

    private readonly IContext _dbcontext;

    public UserManagementService(IContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public Task<bool> CreateUserAsync(string userid)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshTokenAsync(string userid)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetPackageOwner(string id, string userid)
    {
        throw new NotImplementedException();
    }
}

public static class UserManagementServiceExtensions
{
    public static void AddUserManagement(this IServiceCollection c)
    {
        c.AddTransient<IUserManagementService, UserManagementService>();
    }
}
