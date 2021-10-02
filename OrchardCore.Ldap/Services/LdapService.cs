using System;
using System.Linq;
using System.Threading.Tasks;
using Novell.Directory.Ldap;
using OrchardCore.Entities;
using OrchardCore.Ldap.Data;
using OrchardCore.Ldap.Models;
using OrchardCore.Settings;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using YesSql;

namespace OrchardCore.Ldap.Services
{
    public class LdapService: ILdapService
    {
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly ISession _session;
        public LdapService(
            ISiteService siteService,
            IUserService userService,
            ISession session)
        {
            _siteService = siteService;
            _userService = userService;
            _session = session;
        }

        public async Task<bool> CheckLogin(string username, string pass)
        {
            bool res = true;
            var ldapSettings = (await _siteService.GetSiteSettingsAsync()).As<LdapSettings>();
            string ldapHost = ldapSettings.Host;
            string ldapPortStr = ldapSettings.Port;
            string pattern = ldapSettings.Pattern;
            int ldapPort = 0;
            int.TryParse(ldapPortStr, out ldapPort);

            LdapConnection conn = new LdapConnection();
            try
            {
                conn.Connect(ldapHost, ldapPort);
                conn.Bind(@$"{pattern}\{username}", pass);
            }
            catch
            {
                res = false;
            }
            finally
            {
                conn.Disconnect();
            }

            return await Task.FromResult<bool>(res);
        }

        private async Task<UserInfoLdap> GetInfoUserLdap(string username)
        {
            var res = new UserInfoLdap();
            var ldapSettings = (await _siteService.GetSiteSettingsAsync()).As<LdapSettings>();
            string ldapHost = ldapSettings.Host;
            string ldapPortStr = ldapSettings.Port;
            int ldapPort = 0;
            int.TryParse(ldapPortStr, out ldapPort);
            String loginDN = ldapSettings.Account;
            String password = ldapSettings.Pass;
            String searchBase = ldapSettings.Base;
            String searchFilter = $"(&(sAMAccountName={username}))";
            LdapConnection conn = new LdapConnection();
            try
            {
                conn.Connect(ldapHost, ldapPort);
                conn.Bind(loginDN, password);
                LdapSearchResults lsc = conn.Search(searchBase, LdapConnection.ScopeSub, searchFilter, null, false) as LdapSearchResults;
                while (lsc.HasMore())
                {
                    LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.Next();
                    }
                    catch
                    {
                        continue;
                    }
                    LdapAttributeSet attributeSet = nextEntry.GetAttributeSet();
                    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        LdapAttribute attribute = (LdapAttribute)ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        if (attributeName == "displayName")
                        {
                            res.DisplayName = attributeVal;
                        }
                        if (attributeName == "sAMAccountName")
                        {
                            res.AccountName = attributeVal;
                        }
                        if (attributeName == "mail")
                        {
                            res.Email = attributeVal;
                        }
                        if (attributeName == "department")
                        {
                            res.Department = attributeVal;
                        }
                    }
                }
            }
            catch
            {
                res = null;
            }
            finally
            {
                conn.Disconnect();
            }

            return await Task.FromResult<UserInfoLdap>(res);
        }

        public async Task<bool> CreateUserFromLdap(string username, string pass)
        {
            var userInfo = await GetInfoUserLdap(username);
            if(userInfo != null)
            {
                var user = new User();
                user.UserName = username;
                user.NormalizedUserName = username.ToUpper();
                user.Email = userInfo.Email;
                user.NormalizedEmail = userInfo.Email.ToUpper();
                user.EmailConfirmed = false;
                user.IsEnabled = true;
                var iuser = await _userService.CreateUserAsync(user, pass, null);
                if(iuser != null && iuser.UserName != "")
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        public async Task SaveInfoLdap(string username)
        {
            var ldaps = await _session.Query<LdapEntity, LdapIndex>()
                .Where(x => x.UserName == username)
                .ListAsync();
            if (!ldaps.Any())
            {
                var ldap = new LdapEntity
                {
                    UserName = username,
                    CreateDate = System.DateTime.Now
                };
                _session.Save(ldap);
            }
        }

        public string GetPassDefault()
        {
            return "Abc@123";
        }

        public async Task<bool> CheckUserIsLdap(string username)
        {
            var ldaps = await _session.Query<LdapEntity, LdapIndex>()
                .Where(x => x.UserName == username)
                .ListAsync();
            return ldaps.Any();
        }
    }
}
