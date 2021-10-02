using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Entities;

namespace OrchardCore.Ldap.Data2
{
    public class LdapConfig: Entity
    {
        public int Id { get; set; }
        public string Pattern { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Account { get; set; }
        public string Pass { get; set; }
        public string Base { get; set; }
    }
}
