using BaGetter.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace BaGetter.Web.Authorization;

public class CustomTokenAuthenticationHandler : AuthenticationHandler<CustomTokenAuthenticationOptions>
{

    private readonly IContext _context;
    private readonly IOptionsSnapshot<BaGetterOptions> _options;

    public CustomTokenAuthenticationHandler(
        IContext context,
        IOptionsMonitor<CustomTokenAuthenticationOptions> options,
        ILoggerFactory logger,
        IOptionsSnapshot<BaGetterOptions> confOptions,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {

        _options = confOptions ?? throw new ArgumentNullException(nameof(confOptions));
        _context = context;
    }

    private string username = string.Empty;
    private bool isDefaultAdmin = false;
    private bool isAdmin = false;
    private bool isFound = false;

    private void CheckData()
    {
        var token = Request.GetApiKey();
        var userInst = _context.Users.SingleOrDefault(x => x.Token == token);
        if (userInst is not null)
        {
            username = userInst.Username;
            isAdmin = userInst.IsAdmin;
            isFound = true;
            return;
        }

        var confToken = _options.Value.ApiKey;
        if (token == null && string.IsNullOrWhiteSpace(confToken))
        {
            username = "Default";
            isDefaultAdmin = true;
            isAdmin = true;
            isFound = true;
            return;
        }

        if (token == confToken)
        {
            username = "Default";
            isDefaultAdmin = true;
            isAdmin = true;
            isFound = true;
            return;
        }
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        CheckData();
        if (!isFound)
            return Task.FromResult(AuthenticateResult.Fail("Failed to Auth"));

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, username),
        };
        if (isAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        if (isDefaultAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "DefaultAdmin"));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
