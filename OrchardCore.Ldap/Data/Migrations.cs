using System;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace OrchardCore.Ldap.Data
{
    public class Migrations: DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<LdapIndex>(table => table
              .Column<string>("UserName")
              .Column<DateTime?>("CreateDate", c => c.Nullable())
            );
            return 1;
        }
    }
}
