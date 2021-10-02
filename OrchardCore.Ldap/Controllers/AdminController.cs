using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Ldap.Data2;
using OrchardCore.Ldap.Models;
using OrchardCore.Ldap.Services02;
using OrchardCore.Routing;

namespace OrchardCore.Ldap.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILdapConfigService _ldapConfigService;
        public AdminController(ILdapConfigService ldapConfigService)
        {
            _ldapConfigService = ldapConfigService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list =await _ldapConfigService.GetList();
            var first = list.FirstOrDefault();
            var model = new LdapSettingsViewModel();
            if (first != null)
            {
                model.Account = first.Account;
                model.Base = first.Base;
                model.Host = first.Host;
                model.Pass = first.Pass;
                model.Pattern = first.Pattern;
                model.Port = first.Port;
            }
            return View(model);
        }

        [HttpPost, ActionName(nameof(Index))]
        public IActionResult IndexPost(LdapSettingsViewModel model)
        {
            var entity = new LdapConfig
            {
                Account = model.Account,
                Base = model.Base,
                Host = model.Host,
                Pass = model.Pass,
                Pattern = model.Pattern,
                Port = model.Port
            };
            _ldapConfigService.SaveConfig(entity);
            return View(model);
        }
    }
}
