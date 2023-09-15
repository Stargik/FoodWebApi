using System;
namespace FoodMVCWebApp.Entities
{
	public class DifficultyLevel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}

