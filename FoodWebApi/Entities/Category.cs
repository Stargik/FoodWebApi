using System;
namespace FoodMVCWebApp.Entities
{
	public class Category
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public List<Dish> Dishes { get; set; } = new List<Dish>();
	}
}

