using System.Threading;
using System.Threading.Tasks;

namespace BaGetter.Core;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken);

    /// <summary>
    /// Try to authenticate for a specific package
    /// </summary>
    /// <param name="apiKey">API key to check</param>
    /// <param name="packageName">Package name to check</param>
    /// <param name="cancellationToken"></param>
    /// <returns>True if the apiKey's user is authorized to write to it</returns>
    Task<bool> AuthenticateAsync(string apiKey, string packageName, CancellationToken cancellationToken);
}
