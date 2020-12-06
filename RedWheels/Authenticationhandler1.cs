using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace review.Models
{
    public class Authenticationhandler1 : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public Authenticationhandler1(IOptionsMonitor<AuthenticationSchemeOptions> opt,
            ILoggerFactory log, UrlEncoder enc, ISystemClock clock) : base(opt, log, enc, clock)
        {

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("missing header");
            }
            AuthenticationHeaderValue a =
                               AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            string b = a.Parameter;
            string[] arr = b.Split(':');

            //**** check the userName arr[0] and password arr[1] in login table in DB ...
            //   id   |    name   |  password   |  Role
            //  123   |     dani   |   shhhh    |  admin
            //  456   |     dina   |   shhd     | regular

            Claim[] arr2 = new[]{new Claim(ClaimTypes.NameIdentifier,arr[0]),
                                new Claim(ClaimTypes.Name,"admin")};
            ClaimsIdentity i = new ClaimsIdentity(arr2, Scheme.Name);
            ClaimsPrincipal p = new ClaimsPrincipal(i);
            AuthenticationTicket t = new AuthenticationTicket(p, Scheme.Name);            
            
            return AuthenticateResult.Success(t);
        }
    }
}
