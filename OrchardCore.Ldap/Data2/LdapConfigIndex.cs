using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesSql.Indexes;

namespace OrchardCore.Ldap.Data2
{
    public class LdapConfigIndex: MapIndex
    {
        public string Pattern { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Account { get; set; }
        public string Pass { get; set; }
        public string Base { get; set; }
    }

    public class LdapConfigIndexProvider : IndexProvider<LdapConfig>
    {
        public override void Describe(DescribeContext<LdapConfig> context)
        {
            context.For<LdapConfigIndex>()
                .Map(item =>
                {
                    return new LdapConfigIndex
                    {
                        Pattern = item.Pattern,
                        Host = item.Host,
                        Port=item.Port,
                        Account = item.Account,
                        Pass = item.Pass,
                        Base = item.Base
                    };
                });
        }
    }
}
