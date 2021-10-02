using Microsoft.AspNetCore.Http;

namespace OrchardCore.Ldap.Utilities
{
    public static class ClsCommon
    {
        public const string RequestLogin = @"/Login";
        public static bool IsRequestLogin(HttpContext context)
        {
            if(context.Request.Path.StartsWithSegments(RequestLogin)
                && HttpMethods.IsPost(context.Request.Method))
            {
                return true;
            }
            return false;
        }
    }
}
