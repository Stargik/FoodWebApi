using System;
namespace FoodWebApi.Interfaces
{
	public interface ICacheService
	{
		Task<T> GetData<T>(string key);
		Task SetData<T>(string key, T value, TimeSpan duration);
        Task RemoveData(string key);
    }
}

