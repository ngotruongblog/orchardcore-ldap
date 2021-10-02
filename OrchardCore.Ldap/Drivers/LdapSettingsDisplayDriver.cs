using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Ldap.Models;
using OrchardCore.Settings;

namespace OrchardCore.Ldap.Drivers
{
    public class LdapSettingsDisplayDriver : SectionDisplayDriver<ISite, LdapSettings>
    {
        public const string GroupId = "ldap";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public LdapSettingsDisplayDriver(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        public override async Task<IDisplayResult> EditAsync(LdapSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, PermissionLdap.ManageLdapSettings))
            {
                return null;
            }

            return Initialize<LdapSettingsViewModel>("LdapSettings_Edit", model =>
            {
                model.Pattern = settings.Pattern;
                model.Host = settings.Host;
                model.Port = settings.Port;
                model.Account = settings.Account;
                model.Pass = settings.Pass;
                model.Base = settings.Base;

            }).Location("Content:6").OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(LdapSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, PermissionLdap.ManageLdapSettings))
            {
                return null;
            }

            if (context.GroupId == GroupId)
            {
                var model = new LdapSettingsViewModel();
                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Pattern = model.Pattern;
                settings.Host = model.Host;
                settings.Port = model.Port;
                settings.Account = model.Account;
                settings.Pass = model.Pass;
                settings.Base = model.Base;
            }

            return await EditAsync(settings, context);
        }
    }
}
