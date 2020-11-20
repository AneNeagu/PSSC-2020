using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class Tag
    {
        public Tag()
        {
            PostTag = new HashSet<PostTag>();
        }

        public int TenantId { get; set; }
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
    }
}
