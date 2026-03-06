using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Group;

namespace WebAPI.DbConfigurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMember");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.GroupId).IsRequired();
        
        builder.Property(x => x.UserId).IsRequired();
        
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
        
        builder.Property(x => x.JoinedAtUtc).IsRequired();
        
        builder.Property(x => x.Status).HasMaxLength(20).IsRequired();
        
        builder.HasOne(x => x.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();
    }
}