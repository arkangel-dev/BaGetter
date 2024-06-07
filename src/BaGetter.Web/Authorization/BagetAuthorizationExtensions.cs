using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Web.Authorization;
public static class BagetAuthorizationExtensions
{
    public static void AddBagetAuthorization(this IServiceCollection me)
    {
        me.AddAuthentication(o => { o.DefaultScheme = "CustomToken"; })
            .AddScheme<CustomTokenAuthenticationOptions, CustomTokenAuthenticationHandler>("CustomToken", options => { });
        //me
        //    .AddAuthentication(o => { o.DefaultScheme = nameof(BagetAuthentication); })
        //    .AddScheme<BagetAuthenticationOptions, BagetAuthentication>(nameof(BagetAuthentication), null);
    }

    public static void AddBagetAuthentication(this IApplicationBuilder me)
    {
        me.UseAuthentication();
        me.UseAuthorization();
        //me.UseMiddleware<CustomAuthenticationMiddleware>();
    }
}
