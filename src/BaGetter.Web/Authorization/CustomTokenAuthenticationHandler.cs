using BaGetter.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace BaGetter.Web.Authorization;

public class CustomTokenAuthenticationHandler : AuthenticationHandler<CustomTokenAuthenticationOptions>
{

    private IContext _context { get; set; }

    public CustomTokenAuthenticationHandler(
        IContext context,
        IOptionsMonitor<CustomTokenAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
        _context = context;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Your custom authentication logic here
        var authHeader = Request.Headers["Authorization"].ToString();

        //if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Custom ", System.StringComparison.OrdinalIgnoreCase))
        //{
        //    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        //}

        //var token = authHeader.Substring("Custom ".Length).Trim();

        //// Validate the token (this is just an example, replace with your actual logic)
        //if (token != "valid-token")
        //{
        //    return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
        //}

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "user-id"),
            new Claim(ClaimTypes.Name, "username"),
            new Claim(ClaimTypes.Role, "Admin")
            // Add other claims as needed
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
