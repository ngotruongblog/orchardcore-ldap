using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Ldap
{
    public class PermissionLdap : IPermissionProvider
    {
        public static readonly Permission ManageLdapSettings = new Permission("ManageLdapSettings", "Manage Ldap Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                ManageLdapSettings
            }
            .AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageLdapSettings }
                },
            };
        }
    }
}
