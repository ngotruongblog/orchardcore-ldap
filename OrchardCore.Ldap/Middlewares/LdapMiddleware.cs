using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using OrchardCore.Ldap.Services;
using OrchardCore.Users;

namespace OrchardCore.Ldap.Middlewares
{
    public class LdapMiddleware: IMiddleware
    {
        private readonly UserManager<IUser> _userManager;
        private readonly ILdapService _ldapService;
        private readonly SignInManager<IUser> _signInManager;

        public LdapMiddleware(UserManager<IUser> userManager,
            ILdapService ldapService,
            SignInManager<IUser> signInManager)
        {
            _userManager = userManager;
            _ldapService = ldapService;
            _signInManager = signInManager;
        }

        private IFormCollection CreateForm(IFormCollection form, string passRandom)
        {
            Dictionary<string, StringValues> values = new Dictionary<string, StringValues>();
            foreach (var item in form)
            {
                var key = item.Key;
                var value = item.Value;
                if(key == "Password")
                {
                    value = passRandom;
                }
                values.Add(key, value);
            }

            IFormCollection res = new FormCollection(values);
            return res;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string username = string.Empty;
            string pass = string.Empty;
            string passRandom = string.Empty;
            if(context.Request.Form.ContainsKey("UserName") && context.Request.Form.ContainsKey("Password"))
            {
                username = context.Request.Form["UserName"];
                pass = context.Request.Form["Password"];
            }

            var user = await _userManager.FindByNameAsync(username);
            var loginLdap = await _ldapService.CheckLogin(username, pass);
            if (user != null)
            {
                var isUserLdap = await _ldapService.CheckUserIsLdap(username);
                if (isUserLdap == true)
                {
                    if (loginLdap == true)
                    {
                        var change_pass = await HasChangePass(user, pass);
                        if (change_pass == true)
                        {
                            //Reset passwork for user ldap
                            await ChangePass(user, pass);
                        }
                    }
                    else
                    {
                        //Assign random value for passwork
                        passRandom = System.Guid.NewGuid().ToString();
                    }
                }
            }
            else
            {
                if (loginLdap == true)
                {
                    //Add user into user table
                    var flag = await _ldapService.CreateUserFromLdap(username, pass);
                    //Add user into ldap table 
                    if (flag == true)
                    {
                        await _ldapService.SaveInfoLdap(username);
                    }
                }
            }

            if (!string.IsNullOrEmpty(passRandom))
            {
                CreateForm(context.Request.Form, passRandom);
            }

            await next(context);
        }

        private async Task<bool> HasChangePass(IUser user, string password)
        {
            var hasChangePass = false;
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                hasChangePass = true;
            }

            return hasChangePass;
        }

        private async Task<bool> ChangePass(IUser user, string newPass)
        {
            var res = false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var iden = await _userManager.ResetPasswordAsync(user, token, _ldapService.GetPassDefault());
            if (iden.Succeeded)
            {
                var result = await _userManager.ChangePasswordAsync(user, _ldapService.GetPassDefault(), newPass);
                if (result.Succeeded)
                {
                    res = true;
                }
            }

            return res;
        }
    }
}
