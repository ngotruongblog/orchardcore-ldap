using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Ldap.Data2;

namespace OrchardCore.Ldap.Services02
{
    public interface ILdapConfigService
    {
        Task<List<LdapConfig>> GetList();
        Task SaveConfig(LdapConfig ldapConfig);
    }
}
