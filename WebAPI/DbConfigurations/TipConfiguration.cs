using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models;

namespace WebAPI.DbConfigurations;

public class TipConfiguration : IEntityTypeConfiguration<Tip>
{
    public void Configure(EntityTypeBuilder<Tip> builder)
    {
        builder.ToTable("Tips");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MatchId).IsRequired();
        
        builder.Property(x => x.LeagueId).IsRequired();
        
        builder.Property(x => x.PredictedHomeScore).IsRequired();
        builder.Property(x => x.PredictedAwayScore).IsRequired();
        
        builder.Property(x => x.SubmittedAtUtc).IsRequired();
        
        builder.Property(x => x.ResultStatus).HasMaxLength(50).IsRequired();

        builder.Property(x => x.AwardedPoints)
            .IsRequired(false)
            .HasDefaultValue(null);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Tips)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => new { x.UserId, x.MatchId }).IsUnique();
        
        builder.HasIndex(x => x.MatchId);
    }
}