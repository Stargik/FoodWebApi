using System;
using FoodMVCWebApp.Configuration;
using FoodMVCWebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodMVCWebApp.Data
{
	public class FoodDbContext : DbContext
	{
		public FoodDbContext(DbContextOptions options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DifficultyLevelConfiguration());
        }

        public DbSet<Category> Categories { get; set; }
		public DbSet<Dish> Dishes { get; set; }
		public DbSet<CuisineCountryType> CuisineCountryTypes { get; set; }
		public DbSet<DifficultyLevel> DifficultyLevels { get; set; }
	}
}

