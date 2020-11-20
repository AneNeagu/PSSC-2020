using System;
using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class Badge
    {
        public Badge()
        {
            UserBadge = new HashSet<UserBadge>();
        }

        public int BadgeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? PointsRequired { get; set; }

        public virtual ICollection<UserBadge> UserBadge { get; set; }
    }
}
