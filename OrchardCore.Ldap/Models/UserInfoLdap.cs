using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Ldap.Models
{
    public class UserInfoLdap
    {
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
    }
}
