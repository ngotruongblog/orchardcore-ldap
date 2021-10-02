using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrchardCore.Ldap.Models
{
    public class LdapSettings
    {
        public string Pattern { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Account { get; set; }
        public string Pass { get; set; }
        public string Base { get; set; }
    }
}
