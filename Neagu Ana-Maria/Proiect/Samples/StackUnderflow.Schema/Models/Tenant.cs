using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class Tenant
    {
        public Tenant()
        {
            Tag = new HashSet<Tag>();
            TenantUser = new HashSet<TenantUser>();
        }

        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OrganisationId { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime SysStartTime { get; set; }
        public DateTime SysEndTime { get; set; }

        public virtual ICollection<Tag> Tag { get; set; }
        public virtual ICollection<TenantUser> TenantUser { get; set; }
    }
}
