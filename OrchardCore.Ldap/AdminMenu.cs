using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Ldap.Drivers;
using OrchardCore.Navigation;

namespace OrchardCore.Ldap
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }
        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            builder
                 .Add(S["Configuration"], configuration => configuration
                     .Add(S["Settings"], settings => settings
                        .Add(S["Ldap"], S["Ldap"].PrefixPosition(), entry => entry
                        .AddClass("ldap").Id("ldap")
                           .Action("Index", "Admin", new { area = "OrchardCore.Settings", LdapSettingsDisplayDriver.GroupId })
                           .Permission(PermissionLdap.ManageLdapSettings)
                           .LocalNav()
                 )));

            builder
                 .Add(S["Configuration"], configuration => configuration
                     .Add(S["Settings"], settings => settings
                        .Add(S["Ldap02"], S["Ldap02"].PrefixPosition(), entry => entry
                        .AddClass("ldap02").Id("ldap02")
                           .Action("Index", "Admin", new { area = "OrchardCore.Ldap" })
                           .Permission(PermissionLdap.ManageLdapSettings)
                           .LocalNav()
                 )));

            return Task.CompletedTask;
        }
    }
}
