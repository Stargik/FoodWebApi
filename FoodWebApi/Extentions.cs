using System;
using FoodMVCWebApp.Entities;
using FoodMVCWebApp.Models;
using Nest;

namespace FoodWebApi
{
	public static class Extentions
	{
        public static void AddDishElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ESConfiguration:Url"];
            var indexName = configuration["ESConfiguration:Index"];
            //var userName = configuration["ESConfiguration:UserName"];
            //var password = configuration["ESConfiguration:password"];

            var settings = new ConnectionSettings(new Uri(url)) //.BasicAuthentication(userName, password)
                .PrettyJson()
                .DefaultIndex(indexName);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, indexName);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Dish>(m => m
                    .Ignore(d => d.CategoryId)
                    .Ignore(d => d.CuisineCountryTypeId)
                    .Ignore(d => d.DifficultyLevelId)
                    .Ignore(d => d.Image)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Dish>(x => x.AutoMap())
            );
        }
    }
}

