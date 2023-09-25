using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using FoodMVCWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient elasticClient;
        private readonly FoodDbContext _context;

        public SearchController(IElasticClient elasticClient, FoodDbContext context)
        {
            this.elasticClient = elasticClient;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string term)
        {
            var result = await elasticClient.SearchAsync<Dish>(
                             d => d.Query(
                                 q => q.QueryString(
                                     s => s.Query('*' + term + '*')
                                 )));

            return Ok(result.Documents.ToList());
        }

        [HttpGet("ReIndex")]
        public async Task<IActionResult> ReIndex()
        {
            await elasticClient.DeleteByQueryAsync<Dish>(q => q.MatchAll());

            var dishes = (await _context.Dishes.ToListAsync());

            foreach (var dish in dishes)
            {
                await elasticClient.IndexDocumentAsync(dish);
            }

            return Ok($"Dishes reindexed");

        }
    }
}
