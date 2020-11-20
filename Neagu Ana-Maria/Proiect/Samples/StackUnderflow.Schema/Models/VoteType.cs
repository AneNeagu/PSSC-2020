using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class VoteType
    {
        public VoteType()
        {
            Vote = new HashSet<Vote>();
        }

        public int VoteTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DefaultVoteValue { get; set; }

        public virtual ICollection<Vote> Vote { get; set; }
    }
}
