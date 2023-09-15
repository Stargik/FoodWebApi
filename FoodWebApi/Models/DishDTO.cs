using System;
using System.ComponentModel.DataAnnotations;
using FoodMVCWebApp.Entities;

namespace FoodMVCWebApp.Models
{
	public class DishDTO
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Recipe { get; set; }
        public IFormFile? Image { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Difficulty level")]
        public int DifficultyLevelId { get; set; }
        [Display(Name = "Cuisine country")]
        public int CuisineCountryTypeId { get; set; }
    }
}

