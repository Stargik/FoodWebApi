using System;
namespace FoodWebApi.Models
{
	public class PaginationParams
	{
		public int numberPage { get; set; } = 1;
		public int? PageSize { get; set; }

	}
}

