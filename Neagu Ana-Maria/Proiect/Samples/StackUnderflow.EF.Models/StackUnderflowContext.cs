using System;
using Access.Primitives.EFCore.DSL;
using Access.Primitives.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StackUnderflow.EF.Models
{
    public partial class StackUnderflowContext : DbContext
    {
        public StackUnderflowContext()
        {
        }

        public StackUnderflowContext(DbContextOptions<StackUnderflowContext> options)
            : base(options)
        {
        }

        public static Port<StackUnderflowContext> Factory(IServiceProvider sp)
            => from dbContext in DbContextDSL<StackUnderflowContext>.CreateDbContext(sp)
               select dbContext;

        public virtual DbSet<Badge> Badge { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostTag> PostTag { get; set; }
        public virtual DbSet<PostType> PostType { get; set; }
        public virtual DbSet<PostView> PostView { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<TenantUser> TenantUser { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserBadge> UserBadge { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }
        public virtual DbSet<VoteType> VoteType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=ACCESS-1303SF2\\SQL2017;Database=StackUnderflow;Integrated Security=true;");
            }
        }

        protected virtual string DefaultSchema => "base";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema(DefaultSchema);

            modelBuilder.Entity<Badge>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UX_Badge_Name")
                    .IsUnique();

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("ImageURL")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.PostId });

                entity.HasIndex(e => e.PostTypeId)
                    .HasName("FK_Post_PostType");

                entity.HasIndex(e => new { e.ParentPostId, e.TenantId })
                    .HasName("FK_Post_Post");

                entity.HasIndex(e => new { e.TenantId, e.ClosedBy })
                    .HasName("FK_Post_TenantUser8");

                entity.HasIndex(e => new { e.TenantId, e.LastUpdatedBy })
                    .HasName("FK_Post_TenantUser15");

                entity.HasIndex(e => new { e.TenantId, e.PostedBy })
                    .HasName("FK_Post_TenantUser");

                entity.Property(e => e.PostId).ValueGeneratedOnAdd();

                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostText).IsRequired();

                entity.Property(e => e.RowGuid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Ignore(p => p.SysStartTime);
                entity.Ignore(p => p.SysEndTime);

                entity.HasOne(d => d.PostType)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.PostTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_PostType");

                entity.HasOne(d => d.TenantUser)
                    .WithMany(p => p.PostTenantUser)
                    .HasForeignKey(d => new { d.TenantId, d.ClosedBy })
                    .HasConstraintName("FK_Post_TenantUser_ClosedBy");

                entity.HasOne(d => d.TenantUserNavigation)
                    .WithMany(p => p.PostTenantUserNavigation)
                    .HasForeignKey(d => new { d.TenantId, d.LastUpdatedBy })
                    .HasConstraintName("FK_Post_TenantUser_LastUpdatedBy");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.InversePostNavigation)
                    .HasForeignKey(d => new { d.TenantId, d.ParentPostId })
                    .HasConstraintName("FK_Post_Post");

                entity.HasOne(d => d.TenantUser1)
                    .WithMany(p => p.PostTenantUser1)
                    .HasForeignKey(d => new { d.TenantId, d.PostedBy })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_TenantUser");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.QuestionId, e.TagId });

                entity.HasIndex(e => new { e.TagId, e.TenantId })
                    .HasName("FK_PostTag_Tag");

                entity.HasIndex(e => new { e.TenantId, e.QuestionId })
                    .HasName("FK_PostTag_Post");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => new { d.TenantId, d.QuestionId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostTag_Post");

                entity.HasOne(d => d.T)
                    .WithMany(p => p.PostTag)
                    .HasForeignKey(d => new { d.TenantId, d.TagId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostTag_Tag");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UX_PostType_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PostView>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.UserId, e.PostId, e.Viewed });

                entity.HasIndex(e => new { e.TenantId, e.PostId })
                    .HasName("FK_PostView_Post");

                entity.HasIndex(e => new { e.TenantId, e.UserId })
                    .HasName("FK_PostView_TenantUser");

                entity.Property(e => e.Viewed)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostView)
                    .HasForeignKey(d => new { d.TenantId, d.PostId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostView_Post");

                entity.HasOne(d => d.TenantUser)
                    .WithMany(p => p.PostView)
                    .HasForeignKey(d => new { d.TenantId, d.UserId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostView_TenantUser");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.TagId });

                entity.HasIndex(e => e.TenantId)
                    .HasName("FK_Tag_Tenant");

                entity.Property(e => e.TagId).ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Tag)
                    .HasForeignKey(d => d.TenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag_Tenant");
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UX_Tenant_Name")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Ignore(p => p.SysStartTime);
                entity.Ignore(p => p.SysEndTime);

                entity.Property(e => e.RowGuid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TenantUser>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.UserId });

                entity.HasIndex(e => e.TenantId)
                    .HasName("FK_TenantUser_Tenant");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_TenantUser_User");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TenantUser)
                    .HasForeignKey(d => d.TenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TenantUser_Tenant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TenantUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TenantUser_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.LastAccessed).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.RowGuid).HasDefaultValueSql("(newid())");

                entity.Ignore(e => e.SysStartTime);
                entity.Ignore(e => e.SysEndTime);
            });

            modelBuilder.Entity<UserBadge>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.BadgeId, e.UserId });

                entity.HasIndex(e => e.BadgeId)
                    .HasName("FK_UserBadge_Badge");

                entity.HasIndex(e => new { e.TenantId, e.UserId })
                    .HasName("FK_UserBadge_TenantUser");

                entity.Property(e => e.DateEarned)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Badge)
                    .WithMany(p => p.UserBadge)
                    .HasForeignKey(d => d.BadgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBadge_Badge");

                entity.HasOne(d => d.TenantUser)
                    .WithMany(p => p.UserBadge)
                    .HasForeignKey(d => new { d.TenantId, d.UserId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBadge_TenantUser");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => new { e.TenantId, e.QuestionId, e.VoteTypeId, e.UserId });

                entity.HasIndex(e => e.VoteTypeId)
                    .HasName("FK_Vote_VoteType");

                entity.HasIndex(e => new { e.QuestionId, e.TenantId })
                    .HasName("FK_Vote_Post");

                entity.HasIndex(e => new { e.TenantId, e.UserId })
                    .HasName("FK_Vote_TenantUser");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.VoteType)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => d.VoteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_VoteType");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => new { d.TenantId, d.QuestionId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_Post");

                entity.HasOne(d => d.TenantUser)
                    .WithMany(p => p.Vote)
                    .HasForeignKey(d => new { d.TenantId, d.UserId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_TenantUser");
            });

            modelBuilder.Entity<VoteType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UX_VoteType_Name")
                    .IsUnique();

                entity.Property(e => e.VoteTypeId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
