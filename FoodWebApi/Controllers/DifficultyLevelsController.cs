using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using FoodWebApi.Models;
using Newtonsoft.Json;
using FoodWebApi.Interfaces;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyLevelsController : ControllerBase
    {
        private readonly FoodDbContext _context;
        private readonly ICacheService cacheService;

        public DifficultyLevelsController(FoodDbContext context, ICacheService cacheService)
        {
            _context = context;
            this.cacheService = cacheService;
        }

        // GET: api/DifficultyLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DifficultyLevel>>> GetDifficultyLevels([FromQuery] PaginationParams paginationParams)
        {
            if (_context.DifficultyLevels == null)
            {
                return NotFound();
            }

            var levels = await cacheService.GetData<IEnumerable<DifficultyLevel>>(typeof(DifficultyLevel).ToString());
            if (levels is null)
            {
                levels = await _context.DifficultyLevels.ToListAsync();
                await cacheService.SetData(typeof(DifficultyLevel).ToString(), levels, TimeSpan.FromSeconds(60));
            }

            if (paginationParams.PageSize is not null)
            {
                var paginationLevels = PaginationList<DifficultyLevel>.ToPaginationList(levels, paginationParams.numberPage, (int)paginationParams.PageSize);
                var metadata = new
                {
                    paginationLevels.TotalCount,
                    paginationLevels.PageSize,
                    paginationLevels.CurrentPage,
                    paginationLevels.TotalPages,
                    paginationLevels.HasNext,
                    paginationLevels.HasPrevious,
                    nextLink = $"{Url.PageLink()}?numberPage={paginationParams.numberPage + 1}&PageSize={paginationParams.PageSize}"
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(paginationLevels);
            }
            return Ok(levels);
        }

        // GET: api/DifficultyLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DifficultyLevel>> GetDifficultyLevel(int id)
        {
            if (_context.DifficultyLevels == null)
            {
                return NotFound();
            }
            var levels = await cacheService.GetData<IEnumerable<DifficultyLevel>>(typeof(DifficultyLevel).ToString());
            DifficultyLevel difficultyLevel;
            if (levels is not null)
            {
                difficultyLevel = levels.FirstOrDefault(c => c.Id == id);
                if (difficultyLevel is not null)
                {
                    return Ok(difficultyLevel);
                }
            }

            difficultyLevel = await _context.DifficultyLevels.FindAsync(id);

            if (difficultyLevel == null)
            {
                return NotFound();
            }

            return Ok(difficultyLevel);
        }
    }
}
