using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class Vote
    {
        public int TenantId { get; set; }
        public int QuestionId { get; set; }
        public int VoteTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public int VoteValue { get; set; }

        public virtual Post Post { get; set; }
        public virtual TenantUser TenantUser { get; set; }
        public virtual VoteType VoteType { get; set; }
    }
}
