using System;
using FoodMVCWebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodMVCWebApp.Configuration
{
    public class DifficultyLevelConfiguration : IEntityTypeConfiguration<DifficultyLevel>
    {
        public void Configure(EntityTypeBuilder<DifficultyLevel> builder)
        {
            builder.HasData
            (
                new DifficultyLevel
                {
                    Id = 1,
                    Name = "Begginer"
                },
                new DifficultyLevel
                {
                    Id = 2,
                    Name = "Amateur"
                },
                new DifficultyLevel
                {
                    Id = 3,
                    Name = "Professional"
                }
            );
        }
    }
}

