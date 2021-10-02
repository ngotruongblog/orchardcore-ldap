using Microsoft.AspNetCore.Http;
using OrchardCore.Routing;
using System;

namespace OrchardCore.Ldap.Utilities
{
    public static class ClsCommon
    {
        public const string RequestLogin = @"/Login";
        public static bool IsRequestLogin(HttpContext context)
        {
            if(context.Request.Path.StartsWithNormalizedSegments(RequestLogin, StringComparison.OrdinalIgnoreCase)
                && HttpMethods.IsPost(context.Request.Method))
            {
                return true;
            }
            return false;
        }
    }
}
