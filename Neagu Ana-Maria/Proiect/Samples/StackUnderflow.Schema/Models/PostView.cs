using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class PostView
    {
        public int TenantId { get; set; }
        public Guid UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Viewed { get; set; }

        public virtual Post Post { get; set; }
        public virtual TenantUser TenantUser { get; set; }
    }
}
