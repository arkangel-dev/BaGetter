using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BaGetter.Core;

public class ApiKeyAuthenticationService : IAuthenticationService
{
    private IContext context;

    public ApiKeyAuthenticationService(IContext _context)
    {
        context = _context;
    }

    public Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken)
        => Task.FromResult(Authenticate(apiKey));


    /// <summary>
    /// Check if a user is allowed to write to a package
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="packageName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> AuthenticateAsync(string apiKey, string packageName, CancellationToken cancellationToken)
    {
        var userPFetch = context.Users.SingleOrDefault(x => x.Token == apiKey);
        if (userPFetch is null)
            return Task.FromResult(false);
        if (userPFetch.IsAdmin)
            return Task.FromResult(true);
        var package = context.Packages.FirstOrDefault(x => x.Id == packageName);
        if (package is null) return Task.FromResult(true); // Return true if null because new packages need to be added
        if (package.Owner is null) return Task.FromResult(true); // Return true if there is no owner defined
        return Task.FromResult(package.Owner.Token == apiKey);
    }


    /// <summary>
    /// Authenticate by seeing if there is a user who has the api key provided
    /// </summary>
    /// <param name="apiKey">API key to check</param>
    /// <returns></returns>
    private bool Authenticate(string apiKey)
        => context.Users.Any(x => x.Token == apiKey);

}
