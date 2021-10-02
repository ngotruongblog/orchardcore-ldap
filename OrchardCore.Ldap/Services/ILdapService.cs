using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Ldap.Services
{
    public interface ILdapService
    {
        Task<bool> CheckLogin(string username, string pass);
        string GetPassDefault();
        Task<bool> CheckUserIsLdap(string username);
        Task<bool> CreateUserFromLdap(string username, string pass);
        Task SaveInfoLdap(string username);
    }
}
