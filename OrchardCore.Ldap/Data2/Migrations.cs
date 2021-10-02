using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace OrchardCore.Ldap.Data2
{
    public class MigrationLdapConfig: DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<LdapConfigIndex>(table => table
              .Column<string>("Pattern", c => c.Nullable())
              .Column<string>("Host", c => c.Nullable())
              .Column<string>("Port", c => c.Nullable())
              .Column<string>("Account", c => c.Nullable())
              .Column<string>("Pass", c => c.Nullable())
              .Column<string>("Base", c => c.Nullable())
            );

            return 1;
        }
    }
}
