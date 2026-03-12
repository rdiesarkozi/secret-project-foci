using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models.Group;

namespace WebAPI.DbConfigurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired(false);

        builder.Property(x => x.Visibility)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(x => x.JoinCode)
            .HasMaxLength(20);
        
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => x.JoinCode).IsUnique();
    }
}