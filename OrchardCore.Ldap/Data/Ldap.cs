using System;
using OrchardCore.Entities;

namespace OrchardCore.Ldap.Data
{
    public class LdapEntity: Entity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
