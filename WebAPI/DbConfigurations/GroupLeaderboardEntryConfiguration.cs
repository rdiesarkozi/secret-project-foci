using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Group;

namespace WebAPI.DbConfigurations;

public class GroupLeaderboardEntryConfiguration : IEntityTypeConfiguration<GroupLeaderboardEntry>
{
    public void Configure(EntityTypeBuilder<GroupLeaderboardEntry> builder)
    {
        builder.ToTable("GroupLeaderboardEntries");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.GroupId).IsRequired();
        
        builder.Property(x => x.UserId).IsRequired();
        
        builder.Property(x => x.Points).IsRequired();
        
        builder.Property(x => x.Rank).IsRequired();
        
        builder.Property(x => x.Period).IsRequired();
        
        builder.Property(x => x.PeriodKey).IsRequired();
        
        builder.HasOne(x => x.Group)
            .WithMany(g => g.LeaderboardEntries)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => new { x.GroupId, x.UserId }).IsUnique();
        
        builder.HasIndex(x => new { x.GroupId, x.UserId, x.Period, x.PeriodKey }).IsUnique();
    }
}