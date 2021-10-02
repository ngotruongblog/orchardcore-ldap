using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Ldap.Data2;
using YesSql;

namespace OrchardCore.Ldap.Services02
{
    public class LdapConfigService: ILdapConfigService
    {
        private readonly ISession _session;
        public LdapConfigService(ISession session)
        {
            _session = session;
        }

        public async Task<List<LdapConfig>> GetList()
        {
            var ldaps = await _session.Query<LdapConfig, LdapConfigIndex>().ListAsync();
            return ldaps.ToList();
        }

        public Task SaveConfig(LdapConfig ldapConfig)
        {
            _session.Save(ldapConfig);
            return Task.CompletedTask;
        }
    }
}
