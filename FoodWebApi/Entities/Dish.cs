using System;
using System.ComponentModel.DataAnnotations;

namespace FoodMVCWebApp.Entities
{
	public class Dish
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Recipe { get; set; }
		public string Image { get; set; }
		public int CategoryId { get; set; }
		public int DifficultyLevelId { get; set; }
        public int CuisineCountryTypeId { get; set; }

        public Category Category { get; set; } = null!;
        [Display(Name = "Difficulty level")]
        public DifficultyLevel DifficultyLevel { get; set; } = null!;
        [Display(Name = "Cuisine country")]
        public CuisineCountryType CuisineCountryType { get; set; } = null!;
    }
}

