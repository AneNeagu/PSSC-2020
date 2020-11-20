using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class PostTag
    {
        public int TenantId { get; set; }
        public int QuestionId { get; set; }
        public int TagId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag T { get; set; }
    }
}
