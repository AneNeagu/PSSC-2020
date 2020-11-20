using System.Collections.Generic;

namespace StackUnderflow.EF.Models
{
    public partial class PostType
    {
        public PostType()
        {
            Post = new HashSet<Post>();
        }

        public byte PostTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }

    public enum PostTypeValue : byte
    {
        Question = 1,
        Answer = 2,
        Comment = 3
    }
}
