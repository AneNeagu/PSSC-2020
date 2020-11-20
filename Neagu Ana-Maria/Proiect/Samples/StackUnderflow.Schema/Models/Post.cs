using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackUnderflow.EF.Models
{
    public partial class Post
    {
        public Post()
        {
            InversePostNavigation = new HashSet<Post>();
            PostTag = new HashSet<PostTag>();
            PostView = new HashSet<PostView>();
            Vote = new HashSet<Vote>();
        }

        public int TenantId { get; set; }
        public int PostId { get; set; }
        public byte PostTypeId { get; set; }
        public int? ParentPostId { get; set; }
        public string Title { get; set; }
        public string PostText { get; set; }
        public Guid PostedBy { get; set; }
        public bool AcceptedAnswer { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Closed { get; set; }
        public Guid? ClosedBy { get; set; }
        public DateTime? ClosedDate { get; set; }
        public Guid? LastUpdatedBy { get; set; }
        public Guid RowGuid { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Computed)]
        public DateTime SysStartTime { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Computed)]
        public DateTime SysEndTime { get; set; }

        public virtual Post PostNavigation { get; set; }
        public virtual PostType PostType { get; set; }
        public virtual TenantUser TenantUser { get; set; }
        public virtual TenantUser TenantUser1 { get; set; }
        public virtual TenantUser TenantUserNavigation { get; set; }
        public virtual ICollection<Post> InversePostNavigation { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
        public virtual ICollection<PostView> PostView { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }

        //public List<Post> ChildPosts { get; set; }
    }
}
