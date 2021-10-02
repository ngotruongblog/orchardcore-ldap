using Microsoft.AspNetCore.Http;

namespace OrchardCore.Ldap.Utilities
{
    public static class ClsCommon
    {
        public const string MethodPost = "POST";
        public const string RequestLogin = @"/Login";
        public static bool IsRequestLogin(HttpContext context)
        {
            if(context.Request.Path.StartsWithSegments(RequestLogin)
                && context.Request.Method.ToUpper() == MethodPost)
            {
                return true;
            }
            return false;
        }
    }
}
