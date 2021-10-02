using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Ldap.Controllers;
using OrchardCore.Ldap.Data;
using OrchardCore.Ldap.Data2;
using OrchardCore.Ldap.Drivers;
using OrchardCore.Ldap.Middlewares;
using OrchardCore.Ldap.Services;
using OrchardCore.Ldap.Services02;
using OrchardCore.Ldap.Utilities;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using YesSql.Indexes;

namespace OrchardCore.Ldap
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration,
            IOptions<AdminOptions> adminOptions)
        {
            Configuration = configuration;
            _adminOptions = adminOptions.Value;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, LdapSettingsDisplayDriver>();
            services.AddSingleton<IIndexProvider, LdapIndexProvider>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddTransient<LdapMiddleware>();
            services.AddTransient<ILdapService, LdapService>();

            //Ldap Config Bonus
            services.AddSingleton<IIndexProvider, LdapConfigIndexProvider>();
            services.AddTransient<ILdapConfigService, LdapConfigService>();
            services.AddScoped<IDataMigration, MigrationLdapConfig>();
        }
        public override void Configure(IApplicationBuilder builder,
            IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            builder.UseWhen(context => ClsCommon.IsRequestLogin(context), appBuilder =>
            {
                appBuilder.UseLdapMiddleware();
            });
            routes.MapAreaControllerRoute(
                name: "Ldap",
                areaName: "OrchardCore.Ldap",
                pattern: _adminOptions.AdminUrlPrefix + "/Ldap/Index",
                defaults: new { controller = typeof(AdminController).ControllerName(), action = nameof(AdminController.Index) }
            );
        }
    }
}
