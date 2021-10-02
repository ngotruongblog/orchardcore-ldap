using System;
using YesSql.Indexes;

namespace OrchardCore.Ldap.Data
{
    public class LdapIndex: MapIndex
    {
        public string UserName { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class LdapIndexProvider: IndexProvider<LdapEntity>
    {
        public override void Describe(DescribeContext<LdapEntity> context)
        {
            context.For<LdapIndex>()
                .Map(item =>
                {
                    return new LdapIndex
                    {
                        UserName = item.UserName,
                        CreateDate = item.CreateDate
                    };
                });
        }
    }
}
