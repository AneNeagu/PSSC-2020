using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class TenantUser
    {
        public TenantUser()
        {
            PostTenantUser = new HashSet<Post>();
            PostTenantUser1 = new HashSet<Post>();
            PostTenantUserNavigation = new HashSet<Post>();
            PostView = new HashSet<PostView>();
            UserBadge = new HashSet<UserBadge>();
            Vote = new HashSet<Vote>();
        }

        public int TenantId { get; set; }
        public Guid UserId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Post> PostTenantUser { get; set; }
        public virtual ICollection<Post> PostTenantUser1 { get; set; }
        public virtual ICollection<Post> PostTenantUserNavigation { get; set; }
        public virtual ICollection<PostView> PostView { get; set; }
        public virtual ICollection<UserBadge> UserBadge { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
