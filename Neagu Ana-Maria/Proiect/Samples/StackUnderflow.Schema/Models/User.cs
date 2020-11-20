using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class User
    {
        public User()
        {
            TenantUser = new HashSet<TenantUser>();
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime? LastAccessed { get; set; }
        public string DisplayName { get; set; }
        public Guid WorkspaceId { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Biography { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime SysStartTime { get; set; }
        public DateTime SysEndTime { get; set; }

        public virtual ICollection<TenantUser> TenantUser { get; set; }
    }
}
