using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).UseIdentityByDefaultColumn();
        builder.Property(_ => _.Name).HasMaxLength(255).IsRequired();
        builder.Property(_ => _.Document).HasMaxLength(50).IsRequired();
        builder.Property(_ => _.Email).HasMaxLength(255).IsRequired();
        builder.Property(_ => _.Password).HasMaxLength(255).IsRequired();
        builder.Property(_ => _.Type).IsRequired();
        builder.Property(_ => _.Balance).IsRequired().HasDefaultValue(0);
    }
}
