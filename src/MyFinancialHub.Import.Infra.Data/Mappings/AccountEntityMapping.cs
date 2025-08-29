﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyFinancialHub.Import.Infra.Data.Mappings
{
    internal class AccountEntityMapping : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            builder.Property(t => t.IsActive)
                .HasColumnName("active");

            builder.ToTable("accounts");
        }
    }
}