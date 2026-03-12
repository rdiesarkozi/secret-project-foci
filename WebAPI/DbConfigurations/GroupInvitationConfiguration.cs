using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Group;

namespace WebAPI.DbConfigurations;

public class GroupInvitationConfiguration : IEntityTypeConfiguration<GroupInvitation>
{
    public void Configure(EntityTypeBuilder<GroupInvitation> builder)
    {
        builder.ToTable("GroupInvitations");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.InviteEmail).IsRequired();
        
        builder.Property(x => x.Status).IsRequired();
        
        builder.Property(x => x.Token).IsRequired().HasMaxLength(64);
        
        builder.HasOne(x => x.Group)
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.InvitedByUser)
            .WithMany()
            .HasForeignKey(x => x.InvitedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => x.Token).IsUnique();
    }
}